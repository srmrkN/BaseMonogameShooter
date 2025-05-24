using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;

namespace TestGame.Entities;

public class Enemy : GameEntity
{
    private readonly float _speed;
    private Vector2 _direction;
    private GameConfig _config;

    public Enemy(Vector2 position, GameConfig config)
    {
        Position = position;
        _config = config;
        _speed = _config.EnemySpeed;
    }

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)_config.EnemyTextureSize.X,
            (int)_config.EnemyTextureSize.Y);
        var data = new Color[(int)(_config.EnemyTextureSize.X * _config.EnemyTextureSize.Y)];
        var center = _config.EnemyTextureSize.X / 2;
        for (var y = 0; y < _config.EnemyTextureSize.Y; y++)
        for (var x = 0; x < _config.EnemyTextureSize.X; x++)
        {
            var distance = (float)Math.Sqrt(Math.Pow(x - center, 2) + Math.Pow(y - center, 2));
            data[y * (int)_config.EnemyTextureSize.X + x] =
                distance <= _config.EnemyRadius ? Color.Green : Color.Transparent;
        }

        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
        Position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Update(GameTime gameTime, Vector2 playerCenter)
    {
        var toPlayer = playerCenter - (Position + _config.EnemyTextureSize / 2);
        var distance = toPlayer.Length();
        if (distance > 1f) _direction = Vector2.Normalize(toPlayer);
        Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive) spriteBatch.Draw(Texture, Position, Color.White);
    }

    public bool CheckCollisionWithPlayer(Vector2 playerCenter)
    {
        var distance = Vector2.Distance(Position + _config.EnemyTextureSize / 2, playerCenter);
        return distance < _config.PlayerRadius + _config.EnemyRadius;
    }

    public bool CheckCollisionWithBullet(Bullet bullet)
    {
        var distance = Vector2.Distance(Position + _config.EnemyTextureSize / 2, bullet.Position);
        return distance < _config.EnemyRadius + _config.BulletTextureSize.X / 2;
    }
}