using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame.Core;

public class InputHandler
{
    private KeyboardState _currentKeyboardState;
    private MouseState _currentMouseState;
    private KeyboardState _previousKeyboardState;
    private MouseState _previousMouseState;


    public InputHandler(Viewport viewport)
    {
        Viewport = viewport;
    }

    public Vector2 MousePosition => new(_currentMouseState.X, _currentMouseState.Y);

    public Viewport Viewport { get; }

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key);
    }

    public bool IsExitRequested()
    {
        return _currentKeyboardState.IsKeyDown(Keys.Escape);
    }

    public bool IsLeftMouseButtonPressed()
    {
        return _currentMouseState.LeftButton == ButtonState.Pressed;
    }

    public void ResetKeys()
    {
        _previousKeyboardState = _currentKeyboardState;
    }
}