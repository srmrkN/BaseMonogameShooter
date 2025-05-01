using Microsoft.Xna.Framework;

namespace TestGame.Core;

public static class GameConstants
{
    // Player constants
    public const float PlayerRadius = 20f;
    public const float PlayerDefaultSpeed = 200f;
    public const float PlayerSprintSpeed = 350f;

    // Weapon constants
    public const float WeaponOrbitRadius = 35f;
    public const float BulletSpeed = 600f;
    public const float BulletCooldown = 0.1f;

    // Enemy constants
    public const float EnemyRadius = 12f;
    public const float EnemySpeed = 200f;
    public const float EnemySpawnInterval = 0.5f;
    public const int ScorePerEnemy = 10;
    public const int ScorePenaltyPerCollision = 30;

    // Texture constants
    public static readonly Vector2 PlayerTextureSize = new(50, 50);
    public static readonly Vector2 WeaponTextureSize = new(20, 20);
    public static readonly Vector2 BulletTextureSize = new(5, 5);
    public static readonly Vector2 EnemyTextureSize = new(30, 30);
    
    // Main menu constants
    public const int StartButtonWidth = 150;
    public const int StartButtonHeight = 50;
    public const int ExitButtonWidth = 150;
    public const int ExitButtonHeight = 50;
    public const int GapBetweenTwoButtons = 30;
}