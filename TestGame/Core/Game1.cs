using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Entities;
using TestGame.Managers;

namespace TestGame.Core;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private BulletManager _bulletManager;
    private EnemyManager _enemyManager;
    private UIManager _uiManager;
    private MenuManager _menuManager;
    private SoundManager _soundManager;
    private InputHandler _inputHandler;
    private Player _player;
    private SpriteBatch _spriteBatch;
    private Weapon _weapon;
    private GameState _gameState;
    private CollisionMonitor _collisionMonitor;
    private CriticalErrorHandler _criticalErrorHandler;
    private GameConfig _config;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _config = GameConfig.Load("config.json");
        _player = new Player(_config);
        _weapon = new Weapon(_config);
        _bulletManager = new BulletManager(_config);
        _enemyManager = new EnemyManager(_graphics.GraphicsDevice.Viewport, _config);
        _inputHandler = new InputHandler(_graphics.GraphicsDevice.Viewport);
        _uiManager = new UIManager(_player);
        _menuManager = new MenuManager(_config);
        _soundManager = new SoundManager();
        _gameState = GameState.Menu;
        _collisionMonitor = new CollisionMonitor(_player, 2, _config);
        _criticalErrorHandler = new CriticalErrorHandler(_collisionMonitor, _soundManager);
        
        
        _menuManager.OnStartButtonClicked += () =>
        {
            ResetGame();
            _gameState = GameState.Playing;
            
        };
        _menuManager.OnExitButtonClicked += Exit;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _player.LoadContent(GraphicsDevice);
        _weapon.LoadContent(GraphicsDevice);
        _bulletManager.LoadContent(GraphicsDevice);
        _enemyManager.LoadContent(GraphicsDevice);
        _uiManager.LoadContent(Content);
        _menuManager.LoadContent(GraphicsDevice, Content);
        _soundManager.LoadContent(Content);
    }

    private void ResetGame()
    {
        _config = GameConfig.Load("config.json");
        Console.WriteLine(_config.EnemyRadius);
        _player = new Player(_config);
        _bulletManager = new BulletManager(_config);
        _enemyManager = new EnemyManager(_graphics.GraphicsDevice.Viewport, _config);
        _uiManager = new UIManager(_player);
        _player.LoadContent(GraphicsDevice);
        _bulletManager.LoadContent(GraphicsDevice);
        _enemyManager.LoadContent(GraphicsDevice);
        _uiManager.LoadContent(Content);
        _inputHandler.ResetKeys();
        _soundManager.SubscribeToPlayer(_player);
        _collisionMonitor = new CollisionMonitor(_player, 2, _config);
        _criticalErrorHandler = new CriticalErrorHandler(_collisionMonitor, _soundManager);
    }


    protected override void Update(GameTime gameTime)
    {
        // Считываем нажатия
        _inputHandler.Update();
        switch (_gameState)
        {
            case GameState.Menu:
                // Выводим меню
                _menuManager.Update(gameTime);
                break;
            case GameState.Playing:
                if (_inputHandler.IsExitRequested())
                {
                    _gameState = GameState.Menu;
                    _inputHandler.ResetKeys();
                }
                // Обновляем игрока
                _player.Update(gameTime, _inputHandler);
                // Обновляем оружие
                _weapon.Update(gameTime, _player.Position, _inputHandler);
                // Обновляем все пули
                _bulletManager.Update(gameTime, _player.Position, _weapon, _inputHandler);
                // Обновляем всех врагов
                _enemyManager.Update(gameTime, _player.Center, _bulletManager.Bullets, _player);
                _collisionMonitor.Update();
                // Обновляем UI
                _uiManager.Update(gameTime);

                // Не даем выйти на рамки экрана
                _player.ClampToScreen(_graphics.GraphicsDevice.Viewport);
                break;
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_gameState == GameState.Menu ? Color.Black : Color.CornflowerBlue);

        _spriteBatch.Begin();

        switch (_gameState)
        {
            case GameState.Menu:
                _menuManager.Draw(_spriteBatch);
                break;
            case GameState.Playing:
                _player.Draw(_spriteBatch);
                _weapon.Draw(_spriteBatch);
                _bulletManager.Draw(_spriteBatch);
                _enemyManager.Draw(_spriteBatch);
                _uiManager.Draw(_spriteBatch);
                break;
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

