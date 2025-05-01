using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Core;

namespace TestGame.Entities;

public class Player : GameEntity
{
    private float _speed;
    public int Score { get; private set; }
    public event Action<int> OnScoreIncreased;
    public event Action<int> OnScoreDecreased;

    public Player()
    {
        Position = new Vector2(100, 100);
        _speed = GameConstants.PlayerDefaultSpeed;
        Score = 0;
    }

    public void AddScore(int score)
    {
        Score += score;
        OnScoreIncreased?.Invoke(score);
    }

    public void RemoveScore(int score)
    {
        Score -= score;
        OnScoreDecreased?.Invoke(-score);
    }

    public void TakeDamage()
    {
        
    }

    public Vector2 Center => Position + GameConstants.PlayerTextureSize / 2;

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)GameConstants.PlayerTextureSize.X,
            (int)GameConstants.PlayerTextureSize.Y);
        var data = new Color[(int)GameConstants.PlayerTextureSize.X * (int)GameConstants.PlayerTextureSize.Y];

        var centerX = GameConstants.PlayerTextureSize.X / 2;
        var centerY = GameConstants.PlayerTextureSize.Y / 2;

        for (var y = 0; y < GameConstants.PlayerTextureSize.Y; y++)
        for (var x = 0; x < GameConstants.PlayerTextureSize.X; x++)
        {
            var distance = (float)Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
            data[y * (int)GameConstants.PlayerTextureSize.X + x] =
                distance <= GameConstants.PlayerRadius ? Color.White : Color.Transparent;
        }

        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
    }

    public void Update(GameTime gameTime, InputHandler input)
    {
        var movement = Vector2.Zero;
        if (input.IsKeyDown(Keys.A)) movement.X -= 1;
        if (input.IsKeyDown(Keys.D)) movement.X += 1;
        if (input.IsKeyDown(Keys.W)) movement.Y -= 1;
        if (input.IsKeyDown(Keys.S)) movement.Y += 1;

        _speed = input.IsKeyDown(Keys.LeftShift) ? GameConstants.PlayerSprintSpeed : GameConstants.PlayerDefaultSpeed;

        if (movement != Vector2.Zero)
        {
            movement.Normalize();
            Position += movement * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void ClampToScreen(Viewport viewport)
    {
        Position = new Vector2(
            MathHelper.Clamp(Position.X, -5, viewport.Width - GameConstants.PlayerTextureSize.X + 5),
            MathHelper.Clamp(Position.Y, -5, viewport.Height - GameConstants.PlayerTextureSize.Y + 5)
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}