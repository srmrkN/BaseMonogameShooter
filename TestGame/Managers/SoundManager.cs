using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using TestGame.Entities;

namespace TestGame.Managers;

public class SoundManager
{
    private Random _rnd = new();
    private readonly Dictionary<string, SoundEffect> _soundEffects = new();
    private readonly Dictionary<string, Song> _songs = new();
    private float _musicVolume = 0.3f;

    public void LoadContent(ContentManager content)
    {
        _soundEffects.Add("ShootPop1", content.Load<SoundEffect>("SoundEffects/UI/pop1"));
        _soundEffects.Add("ShootPop2", content.Load<SoundEffect>("SoundEffects/UI/pop2"));
        _soundEffects.Add("ShootPop3", content.Load<SoundEffect>("SoundEffects/UI/pop3"));
        _soundEffects.Add("CriticalError", content.Load<SoundEffect>("SoundEffects/UI/sus"));
    }

    public void PlaySound(string soundName, float volume = 0.5f, float pitch = 0, float pan = 0)
    {
        if (_soundEffects.TryGetValue(soundName, out var soundEffect))
        {
            soundEffect.Play(volume, pitch, pan);
        }
    }

    public void PlayMusic(string musicName, bool isRepeating = true)
    {
        if (_songs.TryGetValue(musicName, out var song))
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = _musicVolume;
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(song);
        }
    }

    public void StopMusic()
    {
        MediaPlayer.Stop();
    }
    

    public void SetMusicVolume(float volume)
    {
        _musicVolume = MathHelper.Clamp(volume, 0f, 1f);
        MediaPlayer.Volume = _musicVolume;
    }
    
    public void SubscribeToPlayer(Player player)
    {
        player.OnScoreIncreased += _ =>
        {
            var soundNum = _rnd.Next(1, 4);
            PlaySound($"ShootPop{soundNum}");
        };
        //player.OnScoreDecreased += _ => PlaySound("score_decrease");
    }
}