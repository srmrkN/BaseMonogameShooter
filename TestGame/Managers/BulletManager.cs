using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;
using TestGame.Entities;

namespace TestGame.Managers;

public class BulletManager
{
    private readonly List<Bullet> _bullets = new();
    private Texture2D _bulletTexture;
    private float _timeSinceLastShot;

    public IReadOnlyList<Bullet> Bullets => _bullets.AsReadOnly();

    public void LoadContent(GraphicsDevice graphicsDevice)
    {
        _bulletTexture = new Texture2D(graphicsDevice, (int)GameConstants.BulletTextureSize.X,
            (int)GameConstants.BulletTextureSize.Y);
        var data = new Color[(int)(GameConstants.BulletTextureSize.X * GameConstants.BulletTextureSize.Y)];
        for (var i = 0; i < data.Length; i++) data[i] = new Color(0.4f, 0.3f, 0.5f, 1f);
        _bulletTexture.SetData(data);
    }

    public void Update(GameTime gameTime, Vector2 playerCenter, Weapon weapon, InputHandler input)
    {
        _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (input.IsLeftMouseButtonPressed() && _timeSinceLastShot >= GameConstants.BulletCooldown)
        {
            var bulletDirection = Vector2.Normalize(input.MousePosition - weapon.Position);

            if (Vector2.Distance(input.MousePosition, playerCenter) < GameConstants.PlayerTextureSize.X / 2)
                bulletDirection *= -1;
            var bullet = new Bullet(weapon.Position + bulletDirection * 10f, bulletDirection)
            {
                Texture = _bulletTexture
            };
            _bullets.Add(bullet);
            _timeSinceLastShot = 0f;
        }

        foreach (var bullet in _bullets)
            if (bullet.IsActive)
            {
                bullet.Update(gameTime);
                if (bullet.IsOutOfBounds(input.Viewport)) bullet.IsActive = false;
            }

        _bullets.RemoveAll(b => !b.IsActive);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _bullets) bullet.Draw(spriteBatch);
    }
}