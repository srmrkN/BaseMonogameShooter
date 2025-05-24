using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;
using TestGame.Entities;

namespace TestGame.Managers;

public class MenuManager
{
    private SpriteFont _buttonFont;
    private SpriteFont _titleFont;
    private Vector2 _startButtonPosition;
    private Vector2 _exitButtonPosition;
    private Vector2 _titleTextPosition;
    private Vector2 _startButtonTextPosition;
    private Vector2 _exitButtonTextPosition;
    private Rectangle _startButtonBounds;
    private Rectangle _exitButtonBounds;
    private MouseState _prevMouseState;
    private Color _startButtonColor = Color.White;
    private Color _exitButtonColor = Color.White;
    private Texture2D _buttonTexture;
    private GameConfig _config;

    public MenuManager(GameConfig config)
    {
        _config = config;
    }
    
    public event Action OnStartButtonClicked;
    public event Action OnExitButtonClicked;

    public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _buttonFont = content.Load<SpriteFont>("Fonts/MainMenuFont");
        _titleFont = content.Load<SpriteFont>("Fonts/Font");
        
        _buttonTexture = new Texture2D(graphicsDevice, _config.StartButtonWidth, _config.StartButtonHeight);
        Color[] data = new Color[_config.StartButtonWidth * _config.StartButtonHeight];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _buttonTexture.SetData(data);
        
            // ReSharper disable PossibleLossOfFraction
        _startButtonPosition = new Vector2(  // Посередине экрана, чуть ниже вертикального центра
            x: graphicsDevice.Viewport.Width / 2 - _config.StartButtonWidth / 2,
            y: graphicsDevice.Viewport.Height / 2);
        _exitButtonPosition = new Vector2( // Ниже кнопки старта
            x: _startButtonPosition.X,
            y: _startButtonPosition.Y + _config.StartButtonHeight + _config.GapBetweenTwoButtons);
        
        var titleTextSize = _titleFont.MeasureString("BaseMonogameShooter");
        var startTextSize = _buttonFont.MeasureString("Start");
        var exitTextSize = _buttonFont.MeasureString("Exit");
        
        _titleTextPosition = new Vector2(
            x: graphicsDevice.Viewport.Width / 2 - titleTextSize.X / 2,
            y: graphicsDevice.Viewport.Height / 4 - titleTextSize.Y / 2);
        
        _startButtonTextPosition = new Vector2(
            x: _startButtonPosition.X + _config.StartButtonWidth / 2 - startTextSize.X / 2,
            y: _startButtonPosition.Y + _config.StartButtonHeight / 2 - startTextSize.Y / 2);
        _exitButtonTextPosition = new Vector2(
            x: _exitButtonPosition.X + _config.ExitButtonWidth / 2 - exitTextSize.X / 2,
            y: _exitButtonPosition.Y + _config.ExitButtonHeight / 2 - exitTextSize.Y / 2);
        
        _startButtonBounds = new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, _config.StartButtonWidth, _config.StartButtonHeight);
        _exitButtonBounds = new Rectangle((int)_exitButtonPosition.X, (int)_exitButtonPosition.Y, _config.ExitButtonWidth, _config.ExitButtonHeight);
    }

    public void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var mousePoint = new Point(mouseState.X, mouseState.Y);
        
        _startButtonColor = _startButtonBounds.Contains(mousePoint) ? Color.Gray : Color.White;
        _exitButtonColor = _exitButtonBounds.Contains(mousePoint) ? Color.Gray : Color.White;

        if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
        {
            if (_startButtonBounds.Contains(mousePoint))
            {
                OnStartButtonClicked?.Invoke();
            }

            if (_exitButtonBounds.Contains(mousePoint))
            {
                OnExitButtonClicked?.Invoke();
            }
        }
        
        _prevMouseState = mouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_buttonTexture, _startButtonPosition, _startButtonColor);
        spriteBatch.Draw(_buttonTexture, _exitButtonPosition, _exitButtonColor);
        
        spriteBatch.DrawString(_titleFont, "BaseMonogameShooter", _titleTextPosition, Color.White);
        spriteBatch.DrawString(_buttonFont, "Start", _startButtonTextPosition, Color.Black);
        spriteBatch.DrawString(_buttonFont, "Exit", _exitButtonTextPosition, Color.Black);
    }
    
}