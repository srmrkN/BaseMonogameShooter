using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Entities;

namespace TestGame.Managers;

public class UIManager
{
    private readonly Player _player;
    private float _scoreFlashTimer;
    private SpriteFont _font;
    private Color _flashColor;
    private readonly List<(string text, Vector2 position, float timer, Color color)> _floatingTexts = new();

    public UIManager(Player player)
    {
        _player = player;
        _player.OnScoreIncreased += OnScoreIncreased;
        _player.OnScoreDecreased += OnScoreDecreased;
    }

    private void OnScoreIncreased(int change)
    {
        _scoreFlashTimer = 0.3f;
        _flashColor = Color.Green;
        _floatingTexts.Add(($"+{change}", new Vector2(-10, -20) + _player.Position, 0.3f, Color.Green));
    }

    private void OnScoreDecreased(int change)
    {
        _scoreFlashTimer = 0.3f;
        _flashColor = Color.Red;
        _floatingTexts.Add(($"{change}", new Vector2(-10, -20) + _player.Position, 0.3f, Color.Red));
    }

    public void LoadContent(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/Font");
    }

    public void Update(GameTime gameTime)
    {
        _scoreFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_scoreFlashTimer < 0)
        {
            _scoreFlashTimer = 0;
            _flashColor = Color.Yellow;
        }
        for (int i = _floatingTexts.Count - 1; i >= 0; i--)
        {
            var (text, pos, timer, color) = _floatingTexts[i];
            _floatingTexts[i] = (text, pos + new Vector2(0, -50 * (float)gameTime.ElapsedGameTime.TotalSeconds), timer - (float)gameTime.ElapsedGameTime.TotalSeconds, color);
            if (_floatingTexts[i].timer <= 0) _floatingTexts.RemoveAt(i);
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        Color color = Color.Lerp(Color.Yellow, _flashColor, _scoreFlashTimer / 0.3f);
        spriteBatch.DrawString(_font, $"Score: {_player.Score}", new Vector2(12, 12), Color.Black);
        spriteBatch.DrawString(_font, $"Score: {_player.Score}", new Vector2(10, 10), color);

        foreach (var (text, pos, _, textColor) in _floatingTexts)
        {
            spriteBatch.DrawString(_font, text, pos, textColor * 0.8f);
        }
    }
}