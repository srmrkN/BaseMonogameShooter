using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Entities;

public abstract class GameEntity
{
    public Vector2 Position { get; protected set; }
    public Texture2D Texture { get; set; }
    public bool IsActive { get; set; } = true;

    public abstract void LoadContent(GraphicsDevice graphicsDevice);
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}