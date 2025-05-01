using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;
using TestGame.Entities;

namespace TestGame.Managers;

public class EnemyManager
{
    private readonly List<Enemy> _enemies = new();
    private readonly Random _random = new();
    private Texture2D _enemyTexture;
    private float _timeSinceLastSpawn;
    private Viewport _viewport;


    public EnemyManager(Viewport viewport)
    {
        _viewport = viewport;
    }

    public void LoadContent(GraphicsDevice graphicsDevice)
    {
        _enemyTexture = new Texture2D(graphicsDevice, (int)GameConstants.EnemyTextureSize.X,
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

        _enemyTexture.SetData(data);
    }

    public void Update(GameTime gameTime, Vector2 playerCenter, IReadOnlyList<Bullet> bullets, Player player, UIManager ui)
    {
        _timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Спавн врагов
        if (_timeSinceLastSpawn >= GameConstants.EnemySpawnInterval)
        {
            Vector2 spawnPosition;
            var edge = _random.Next(4);
            if (edge == 0) // Верх
                spawnPosition = new Vector2(_random.Next(0, _viewport.Width), -GameConstants.EnemyTextureSize.Y);
            else if (edge == 1) // Низ
                spawnPosition = new Vector2(_random.Next(0, _viewport.Width), _viewport.Height);
            else if (edge == 2) // Лево
                spawnPosition = new Vector2(-GameConstants.EnemyTextureSize.X, _random.Next(0, _viewport.Height));
            else // Право
                spawnPosition = new Vector2(_viewport.Width, _random.Next(0, _viewport.Height));

            var enemy = new Enemy(spawnPosition) { Texture = _enemyTexture };
            _enemies.Add(enemy);
            _timeSinceLastSpawn = 0f;
        }

        // Обновление врагов и проверка столкновений
        foreach (var enemy in _enemies)
            if (enemy.IsActive)
            {
                enemy.Update(gameTime, playerCenter);
                if (enemy.CheckCollisionWithPlayer(playerCenter))
                {
                    enemy.IsActive = false;
                    player.RemoveScore(GameConstants.ScorePenaltyPerCollision);
                }
                foreach (var bullet in bullets)
                    if (bullet.IsActive && enemy.CheckCollisionWithBullet(bullet))
                    {
                        enemy.IsActive = false;
                        bullet.IsActive = false;
                        player.AddScore(GameConstants.ScorePerEnemy);
                        break;
                    }
            }

        _enemies.RemoveAll(e => !e.IsActive);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var enemy in _enemies) enemy.Draw(spriteBatch);
    }
}