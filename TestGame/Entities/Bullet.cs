using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;

namespace TestGame.Entities;

public class Bullet : GameEntity
{
    private readonly float _speed;
    private readonly Vector2 _direction;
    private readonly GameConfig _config;

    public Bullet(Vector2 position, Vector2 direction, GameConfig config)
    {
        Position = position;
        _direction = direction;
        _config = config;
        _speed = _config.BulletSpeed;
    }

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)_config.BulletTextureSize.X,
            (int)_config.BulletTextureSize.Y);
        var data = new Color[(int)(_config.BulletTextureSize.X * _config.BulletTextureSize.Y)];
        for (var i = 0; i < data.Length; i++) data[i] = new Color(0.4f, 0.3f, 0.5f, 1f);
        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
        Position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive) spriteBatch.Draw(Texture, Position, Color.White);
    }

    public bool IsOutOfBounds(Viewport viewport)
    {
        return Position.X < 0 || Position.X > viewport.Width || Position.Y < 0 || Position.Y >= viewport.Height;
    }
}