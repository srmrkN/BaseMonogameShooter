using System;
using TestGame.Entities;
using TestGame.Core;

namespace TestGame.Core;

public class CollisionMonitor
{
    private readonly Player _player;
    private int _lastCollisionCount = 0;
    private bool _errorDialogShown = false;
    private readonly int _collisionLimit;
    public event Action OnCriticalError;
    private GameConfig _config;

    public CollisionMonitor(Player player, int collisionLimit, GameConfig config)
    {
        _player = player;
        _collisionLimit = collisionLimit;
        _config = config;
    }

    public void Update()
    {
        int collisionsNow = -_player.Score / _config.ScorePenaltyPerCollision;
        if (collisionsNow <= _lastCollisionCount) return;
        _lastCollisionCount = collisionsNow;
        if (_lastCollisionCount >= _collisionLimit && !_errorDialogShown)
        {
            _errorDialogShown = true;
            OnCriticalError?.Invoke();
        }
    }

    public void Reset()
    {
        _lastCollisionCount = 0;
        _errorDialogShown = false;
    }
}

