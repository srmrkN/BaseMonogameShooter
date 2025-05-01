using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using TestGame.Entities;
using TestGame.Managers;

namespace TestGame.Core;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private BulletManager _bulletManager;
    private EnemyManager _enemyManager;
    private UIManager _uiManager;
    private InputHandler _inputHandler;
    private Player _player;
    private SpriteBatch _spriteBatch;
    private Weapon _weapon;
    private Color _screenColor = Color.CornflowerBlue;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _player = new Player();
        _weapon = new Weapon();
        _bulletManager = new BulletManager();
        _enemyManager = new EnemyManager(_graphics.GraphicsDevice.Viewport);
        _inputHandler = new InputHandler(_graphics.GraphicsDevice.Viewport);
        _uiManager = new UIManager(_player);
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
    }


    protected override void Update(GameTime gameTime)
    {
        if (_inputHandler.IsExitRequested()) Exit();

        _inputHandler.Update();

        _player.Update(gameTime, _inputHandler);
        _weapon.Update(gameTime, _player.Position, _inputHandler);
        _bulletManager.Update(gameTime, _player.Position, _weapon, _inputHandler);
        _enemyManager.Update(gameTime, _player.Center, _bulletManager.Bullets, _player);
        _uiManager.Update(gameTime);

        _player.ClampToScreen(_graphics.GraphicsDevice.Viewport);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_screenColor);

        _spriteBatch.Begin();
        _player.Draw(_spriteBatch);
        _weapon.Draw(_spriteBatch);
        _bulletManager.Draw(_spriteBatch);
        _enemyManager.Draw(_spriteBatch);
        _uiManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}