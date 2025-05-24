using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;

namespace TestGame.Entities;

public class Weapon : GameEntity
{
    private float _rotation;
    private GameConfig _config;

    public Weapon(GameConfig config)
    {
        _config = config;
    }

    public Vector2 Direction =>
        new(MathF.Cos(_rotation - MathHelper.PiOver2), MathF.Sin(_rotation - MathHelper.PiOver2));

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)_config.WeaponTextureSize.X,
            (int)_config.WeaponTextureSize.Y);
        var data = new Color[(int)(_config.WeaponTextureSize.X * _config.WeaponTextureSize.Y)];
        for (var y = 0; y < _config.WeaponTextureSize.Y; y++)
        for (var x = 0; x < _config.WeaponTextureSize.X; x++)
            if (y > x / 2 && y > (_config.WeaponTextureSize.X - x) / 2 && y < 15)
                data[y * (int)_config.WeaponTextureSize.X + x] = Color.Red;
            else
                data[y * (int)_config.WeaponTextureSize.X + x] = Color.Transparent;

        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public void Update(GameTime gameTime, Vector2 playerPosition, InputHandler input)
    {
        var playerCenter = playerPosition + _config.PlayerTextureSize / 2;
        var mousePosition = input.MousePosition;
        var direction = mousePosition - playerCenter;
        var orbitAngle = (float)Math.Atan2(direction.Y, direction.X);
        _rotation = orbitAngle + MathHelper.PiOver2;
        Position = playerCenter + new Vector2(
            MathF.Cos(orbitAngle) * _config.WeaponOrbitRadius,
            MathF.Sin(orbitAngle) * _config.WeaponOrbitRadius
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            null,
            Color.White,
            _rotation,
            new Vector2(_config.WeaponTextureSize.X / 2, 15),
            1f,
            SpriteEffects.None,
            0f
        );
    }
}