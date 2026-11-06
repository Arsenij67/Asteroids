using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Asteroid.Generation;

namespace Asteroid.Audio
{
    public class AudioServiceLocator : IAudioService
    {
        private AudioData _audioData;
        private AudioSource _musicSource;
        private IInstanceCreator _instanceCreator;
        private int _currentPoolIndex = 0;
        private IResourceLoader _resourceLoader;
        private readonly List<AudioSource> _sfxPool = new List<AudioSource>();

        public void Initialize(AudioData audioData, IInstanceCreator instanceCreator,IResourceLoader resourceLoader, ushort poolSize)
        {
            _instanceCreator = instanceCreator;
            _resourceLoader = resourceLoader;
            _audioData = audioData;

            var sfxSources = FillUpAudioPull(poolSize);
            _sfxPool.Clear();
            _sfxPool.AddRange(sfxSources);

            foreach (var source in _sfxPool)
            {
                source.playOnAwake = false;
                source.volume = _audioData.SfxVolume;
            }
            _musicSource = _sfxPool.First();
            _sfxPool.Remove(_sfxPool.First());
        }

        public void PlayLaserShoot()
        {
            PlaySFX(_audioData.LaserShootClip);
        }

        public void PlayExplosion()
        {
            PlaySFX(_audioData.ExplosionClip);
        }

        public void PlayBackgroundMusic()
        {
            if (!_musicSource.isPlaying)
            {
                _musicSource.Play();
                _musicSource.loop = true;
            }
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

        public void Dispose()
        {
            _sfxPool.Clear();
        }

        public void PlayFireballShoot()
        {
            PlaySFX(_audioData.FireballShootClip);
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

        private List<AudioSource> FillUpAudioPull(ushort poolSize)
        {
            var sfxPool = _instanceCreator.CreateInstance<List<AudioSource>>();
            for (int i = 0; i < poolSize; i++)
            {
                var sourceObj = _resourceLoader.Instantiate(_audioData.AudioSourcePrefab,
                    _audioData.AudioSourcePrefab.transform);
                sourceObj.name = $"SFX_Source_{i}";
                if (sourceObj.TryGetComponent<AudioSource>(out AudioSource voice))
                {
                    sfxPool.Add(voice);
                }
            }
            return sfxPool;
        }
    }
}
