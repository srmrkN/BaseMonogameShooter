using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Core;

namespace TestGame.Entities;

public class Weapon : GameEntity
{
    private float _rotation;

    public Vector2 Direction =>
        new(MathF.Cos(_rotation - MathHelper.PiOver2), MathF.Sin(_rotation - MathHelper.PiOver2));

    public override void LoadContent(GraphicsDevice graphicsDevice)
    {
        Texture = new Texture2D(graphicsDevice, (int)GameConstants.WeaponTextureSize.X,
            (int)GameConstants.WeaponTextureSize.Y);
        var data = new Color[(int)(GameConstants.WeaponTextureSize.X * GameConstants.WeaponTextureSize.Y)];
        for (var y = 0; y < GameConstants.WeaponTextureSize.Y; y++)
        for (var x = 0; x < GameConstants.WeaponTextureSize.X; x++)
            if (y > x / 2 && y > (GameConstants.WeaponTextureSize.X - x) / 2 && y < 15)
                data[y * (int)GameConstants.WeaponTextureSize.X + x] = Color.Red;
            else
                data[y * (int)GameConstants.WeaponTextureSize.X + x] = Color.Transparent;

        Texture.SetData(data);
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public void Update(GameTime gameTime, Vector2 playerPosition, InputHandler input)
    {
        var playerCenter = playerPosition + GameConstants.PlayerTextureSize / 2;
        var mousePosition = input.MousePosition;
        var direction = mousePosition - playerCenter;
        var orbitAngle = (float)Math.Atan2(direction.Y, direction.X);
        _rotation = orbitAngle + MathHelper.PiOver2;
        Position = playerCenter + new Vector2(
            MathF.Cos(orbitAngle) * GameConstants.WeaponOrbitRadius,
            MathF.Sin(orbitAngle) * GameConstants.WeaponOrbitRadius
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
            new Vector2(GameConstants.WeaponTextureSize.X / 2, 15),
            1f,
            SpriteEffects.None,
            0f
        );
    }
}