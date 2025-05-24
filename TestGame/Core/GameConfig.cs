using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace TestGame.Core;

public class GameConfig
{
    public float PlayerRadius { get; set; } = 20f;
    public float PlayerDefaultSpeed { get; set; } = 200f;
    public float PlayerSprintSpeed { get; set; } = 350f;

    // Weapon constants
    public float WeaponOrbitRadius { get; set; } = 35f;
    public float BulletSpeed { get; set; } = 600f;
    public float BulletCooldown { get; set; } = 0.1f;

    // Enemy constants
    public float EnemyRadius { get; set; } = 12f;
    public float EnemySpeed { get; set; } = 200f;
    public float EnemySpawnInterval { get; set; } = 0.5f;
    public int ScorePerEnemy { get; set; } = 10;
    public int ScorePenaltyPerCollision { get; set; } = 30;

    // Texture constants
    public Vector2 PlayerTextureSize { get; set; } = new(50, 50);
    public Vector2 WeaponTextureSize { get; set; } = new(20, 20);
    public Vector2 BulletTextureSize { get; set; } = new(5, 5);
    public Vector2 EnemyTextureSize { get; set; } = new(30, 30);

    // Main menu constants
    public int StartButtonWidth { get; set; } = 150;
    public int StartButtonHeight { get; set; } = 50;
    public int ExitButtonWidth { get; set; } = 150;
    public int ExitButtonHeight { get; set; } = 50;
    public int GapBetweenTwoButtons { get; set; } = 30;

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            // Гарантируем, что конвертер будет использован при чтении и записи
            Converters = { new Vector2JsonConverter() } 
        };
        return options;
    }

    public static GameConfig Load(string path)
    {
        GameConfig configInstance;
        if (File.Exists(path))
        {
            Console.WriteLine("Loading configuration from: " + path);
            try
            {
                string json = File.ReadAllText(path);
                configInstance = JsonSerializer.Deserialize<GameConfig>(json, GetJsonSerializerOptions());
                
                if (configInstance == null)
                {
                    Console.WriteLine("Deserialized config is null (e.g., JSON was 'null'). Using default configuration and saving.");
                    configInstance = new GameConfig();
                    configInstance.Save(path); // Сохраняем, так как исходный файл был некорректен для объекта
                    return configInstance; // Возвращаем свежий дефолтный конфиг
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing config file '{path}': {ex.Message}. A new default config will be created and used.");
                configInstance = new GameConfig();
                configInstance.Save(path); // Сохраняем свежий дефолтный конфиг из-за ошибки
                return configInstance; // Возвращаем, валидация не нужна для свежего дефолтного
            }
        }
        else
        {
            Console.WriteLine($"Configuration file not found: {path}. Creating default configuration.");
            configInstance = new GameConfig();
            configInstance.Save(path);
            return configInstance; // Возвращаем, валидация не нужна для свежего дефолтного
        }

        // Проводим валидацию и коррекцию загруженных значений
        configInstance.ValidateAndCorrect(path); // Этот метод сохранит конфиг, если были внесены исправления
        return configInstance;
    }

    public void Save(string path)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(this, GetJsonSerializerOptions()));
    }

    private void LogCorrection(string propertyName, object oldValue, object newValue)
    {
        Console.WriteLine($"Warning: Invalid value for {propertyName} ('{oldValue}'). Resetting to default: '{newValue}'.");
    }

    /// <summary>
    /// Validates loaded configuration values and corrects them if necessary.
    /// Saves the configuration file if any corrections were made.
    /// </summary>
    /// <param name="configPath">Path to the configuration file for re-saving if needed.</param>
    private void ValidateAndCorrect(string configPath)
    {
        bool corrected = false;
        var defaults = new GameConfig(); // Эталонные значения по умолчанию

        // Player
        if (PlayerRadius <= 0f) { LogCorrection(nameof(PlayerRadius), PlayerRadius, defaults.PlayerRadius); PlayerRadius = defaults.PlayerRadius; corrected = true; }
        if (PlayerDefaultSpeed <= 0f) { LogCorrection(nameof(PlayerDefaultSpeed), PlayerDefaultSpeed, defaults.PlayerDefaultSpeed); PlayerDefaultSpeed = defaults.PlayerDefaultSpeed; corrected = true; }
        if (PlayerSprintSpeed <= 0f) { LogCorrection(nameof(PlayerSprintSpeed), PlayerSprintSpeed, defaults.PlayerSprintSpeed); PlayerSprintSpeed = defaults.PlayerSprintSpeed; corrected = true; }

        // Weapon
        if (WeaponOrbitRadius <= 0f) { LogCorrection(nameof(WeaponOrbitRadius), WeaponOrbitRadius, defaults.WeaponOrbitRadius); WeaponOrbitRadius = defaults.WeaponOrbitRadius; corrected = true; }
        if (BulletSpeed <= 0f) { LogCorrection(nameof(BulletSpeed), BulletSpeed, defaults.BulletSpeed); BulletSpeed = defaults.BulletSpeed; corrected = true; }
        if (BulletCooldown <= 0f) { LogCorrection(nameof(BulletCooldown), BulletCooldown, defaults.BulletCooldown); BulletCooldown = defaults.BulletCooldown; corrected = true; }

        // Enemy
        if (EnemyRadius <= 0f) { LogCorrection(nameof(EnemyRadius), EnemyRadius, defaults.EnemyRadius); EnemyRadius = defaults.EnemyRadius; corrected = true; }
        if (EnemySpeed <= 0f) { LogCorrection(nameof(EnemySpeed), EnemySpeed, defaults.EnemySpeed); EnemySpeed = defaults.EnemySpeed; corrected = true; }
        if (EnemySpawnInterval <= 0f) { LogCorrection(nameof(EnemySpawnInterval), EnemySpawnInterval, defaults.EnemySpawnInterval); EnemySpawnInterval = defaults.EnemySpawnInterval; corrected = true; }
        // ScorePerEnemy и ScorePenaltyPerCollision могут быть 0, но не отрицательными
        if (ScorePerEnemy < 0) { LogCorrection(nameof(ScorePerEnemy), ScorePerEnemy, defaults.ScorePerEnemy); ScorePerEnemy = defaults.ScorePerEnemy; corrected = true; }
        if (ScorePenaltyPerCollision < 0) { LogCorrection(nameof(ScorePenaltyPerCollision), ScorePenaltyPerCollision, defaults.ScorePenaltyPerCollision); ScorePenaltyPerCollision = defaults.ScorePenaltyPerCollision; corrected = true; }

        // Texture constants (размеры должны быть положительными)
        if (PlayerTextureSize.X <= 0 || PlayerTextureSize.Y <= 0) { LogCorrection(nameof(PlayerTextureSize), PlayerTextureSize, defaults.PlayerTextureSize); PlayerTextureSize = defaults.PlayerTextureSize; corrected = true; }
        if (WeaponTextureSize.X <= 0 || WeaponTextureSize.Y <= 0) { LogCorrection(nameof(WeaponTextureSize), WeaponTextureSize, defaults.WeaponTextureSize); WeaponTextureSize = defaults.WeaponTextureSize; corrected = true; }
        if (BulletTextureSize.X <= 0 || BulletTextureSize.Y <= 0) { LogCorrection(nameof(BulletTextureSize), BulletTextureSize, defaults.BulletTextureSize); BulletTextureSize = defaults.BulletTextureSize; corrected = true; }
        if (EnemyTextureSize.X <= 0 || EnemyTextureSize.Y <= 0) { LogCorrection(nameof(EnemyTextureSize), EnemyTextureSize, defaults.EnemyTextureSize); EnemyTextureSize = defaults.EnemyTextureSize; corrected = true; }

        // Main menu constants (размеры должны быть положительными)
        if (StartButtonWidth <= 0) { LogCorrection(nameof(StartButtonWidth), StartButtonWidth, defaults.StartButtonWidth); StartButtonWidth = defaults.StartButtonWidth; corrected = true; }
        if (StartButtonHeight <= 0) { LogCorrection(nameof(StartButtonHeight), StartButtonHeight, defaults.StartButtonHeight); StartButtonHeight = defaults.StartButtonHeight; corrected = true; }
        if (ExitButtonWidth <= 0) { LogCorrection(nameof(ExitButtonWidth), ExitButtonWidth, defaults.ExitButtonWidth); ExitButtonWidth = defaults.ExitButtonWidth; corrected = true; }
        if (ExitButtonHeight <= 0) { LogCorrection(nameof(ExitButtonHeight), ExitButtonHeight, defaults.ExitButtonHeight); ExitButtonHeight = defaults.ExitButtonHeight; corrected = true; }
        // GapBetweenTwoButtons может быть 0, но не отрицательным
        if (GapBetweenTwoButtons < 0) { LogCorrection(nameof(GapBetweenTwoButtons), GapBetweenTwoButtons, defaults.GapBetweenTwoButtons); GapBetweenTwoButtons = defaults.GapBetweenTwoButtons; corrected = true; }

        if (corrected)
        {
            Console.WriteLine("Configuration issues found and corrected. Saving updated config to: " + configPath);
            Save(configPath);
        }
    }
}