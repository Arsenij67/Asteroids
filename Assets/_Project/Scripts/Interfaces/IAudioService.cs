
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroid.Audio
{
    public interface IAudioService : IDisposable
    {
        public void Initialize(AudioData audioData, List<AudioSource> sfxSources);
        public void PlayShoot();
        public void PlayExplosion();
        public void PlayBackgroundMusic();
        public void SetMusicVolume(float volume);
        public void SetExplosionVolume(float volume);
    }
}