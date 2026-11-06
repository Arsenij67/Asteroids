using System;
using System.Collections.Generic;
using UnityEngine;
using Asteroid.Generation;

namespace Asteroid.Audio
{
    public interface IAudioService : IDisposable
    {
        public void Initialize(AudioData audioData, IInstanceCreator instanceCreator, IResourceLoader resourceLoader, ushort poolSize);
        public void PlayLaserShoot();
        public void PlayFireballShoot();
        public void PlayExplosion();
        public void PlayBackgroundMusic();
        public void SetMusicVolume(float volume);
        public void SetExplosionVolume(float volume);
    }
}