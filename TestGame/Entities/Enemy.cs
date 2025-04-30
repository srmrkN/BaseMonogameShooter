using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;

namespace TestGame.Entities;

public class Enemy : GameEntity
{
    private readonly float _speed;
    private Vector2 _direction;

    public Enemy(Vector2 position)
    {
        Position = position;
        _speed = GameConstants.EnemySpeed;
    }

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)GameConstants.EnemyTextureSize.X,
            (int)GameConstants.EnemyTextureSize.Y);
        var data = new Color[(int)(GameConstants.EnemyTextureSize.X * GameConstants.EnemyTextureSize.Y)];
        var center = GameConstants.EnemyTextureSize.X / 2;
        for (var y = 0; y < GameConstants.EnemyTextureSize.Y; y++)
        for (var x = 0; x < GameConstants.EnemyTextureSize.X; x++)
        {
            var distance = (float)Math.Sqrt(Math.Pow(x - center, 2) + Math.Pow(y - center, 2));
            data[y * (int)GameConstants.EnemyTextureSize.X + x] =
                distance <= GameConstants.EnemyRadius ? Color.Green : Color.Transparent;
        }

        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
        Position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Update(GameTime gameTime, Vector2 playerCenter)
    {
        var toPlayer = playerCenter - (Position + GameConstants.EnemyTextureSize / 2);
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
        var distance = Vector2.Distance(Position + GameConstants.EnemyTextureSize / 2, playerCenter);
        return distance < GameConstants.PlayerRadius + GameConstants.EnemyRadius;
    }

    public bool CheckCollisionWithBullet(Bullet bullet)
    {
        var distance = Vector2.Distance(Position + GameConstants.EnemyTextureSize / 2, bullet.Position);
        return distance < GameConstants.EnemyRadius + GameConstants.BulletTextureSize.X / 2;
    }
}