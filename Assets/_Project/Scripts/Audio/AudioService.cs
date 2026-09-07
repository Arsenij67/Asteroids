using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Asteroid.Audio
{
    public class AudioService : IAudioService
    {
        private AudioData _audioData;
        private AudioSource _musicSource;
        private int _currentPoolIndex = 0;

        private readonly List<AudioSource> _sfxPool = new List<AudioSource>();

        public void Initialize(AudioData audioData, List<AudioSource> sfxSources)
        {
            _audioData = audioData;

            _sfxPool.Clear();
            _sfxPool.AddRange(sfxSources);

            foreach (var source in _sfxPool)
            {
                source.playOnAwake = false;
                source.volume = _audioData.SfxVolume;
            }
            _musicSource = _sfxPool.First();
        }

        public void PlayShoot()
        {
            PlaySFX(_audioData.ShootClip);
        }

        public void PlayExplosion()
        {
            PlaySFX(_audioData.ExplosionClip);
        }

        public void PlayBackgroundMusic()
        {
            if (!_musicSource.isPlaying)
                _musicSource.Play();
        }

        public void SetMusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void SetExplosionVolume(float volume)
        {
            foreach (var source in _sfxPool)
            {
                source.volume = volume;
            }
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip == null || _sfxPool.Count == 0) return;

            var source = _sfxPool[_currentPoolIndex];
            _currentPoolIndex = (_currentPoolIndex + 1) % _sfxPool.Count;

            source.clip = clip;
            source.Stop();
            source.Play();
        }
    }
}
