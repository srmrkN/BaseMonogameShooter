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
    private GameConfig _config;


    public EnemyManager(Viewport viewport, GameConfig config)
    {
        _config = config;
        _viewport = viewport;
    }

    public void LoadContent(GraphicsDevice graphicsDevice)
    {
        _enemyTexture = new Texture2D(graphicsDevice, (int)_config.EnemyTextureSize.X,
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

        _enemyTexture.SetData(data);
    }

    public void Update(GameTime gameTime, Vector2 playerCenter, IReadOnlyList<Bullet> bullets, Player player)
    {
        _timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Спавн врагов
        if (_timeSinceLastSpawn >= _config.EnemySpawnInterval)
        {
            Vector2 spawnPosition;
            var edge = _random.Next(4);
            if (edge == 0) // Верх
                spawnPosition = new Vector2(_random.Next(0, _viewport.Width), -_config.EnemyTextureSize.Y);
            else if (edge == 1) // Низ
                spawnPosition = new Vector2(_random.Next(0, _viewport.Width), _viewport.Height);
            else if (edge == 2) // Лево
                spawnPosition = new Vector2(-_config.EnemyTextureSize.X, _random.Next(0, _viewport.Height));
            else // Право
                spawnPosition = new Vector2(_viewport.Width, _random.Next(0, _viewport.Height));

            var enemy = new Enemy(spawnPosition, _config) { Texture = _enemyTexture };
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
                    player.RemoveScore(_config.ScorePenaltyPerCollision);
                }
                foreach (var bullet in bullets)
                    if (bullet.IsActive && enemy.CheckCollisionWithBullet(bullet))
                    {
                        enemy.IsActive = false;
                        bullet.IsActive = false;
                        player.AddScore(_config.ScorePerEnemy);
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