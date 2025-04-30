using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using TestGame.Entities;

namespace TestGame.Managers;

public class UIManager
{
    private readonly Player _player;
    private float _scoreDecreaseFlashTimer;
    private SpriteFont _font;

    public UIManager(Player player)
    {
        _player = player;
    }

    public void NotifyScoreDecreased()
    {
        _scoreDecreaseFlashTimer = 0.1f;
    }

    public void LoadContent(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Font");
    }

    public void Update(GameTime gameTime)
    {
        _scoreDecreaseFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_scoreDecreaseFlashTimer <= 0) _scoreDecreaseFlashTimer = 0;
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color color = _scoreDecreaseFlashTimer > 0 ? Color.Red : Color.Yellow;
        spriteBatch.DrawString(_font, $"Score: {_player.Score}", new Vector2(12, 12), Color.Black);
        spriteBatch.DrawString(_font, $"Score: {_player.Score}", new Vector2(10, 10), color);
    }
}