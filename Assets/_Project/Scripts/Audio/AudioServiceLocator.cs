using UnityEngine;
using System.Collections.Generic;
using Asteroid.Generation;

namespace Asteroid.Audio
{
    public class AudioServiceLocator : IAudioService
    {
        private AudioData _audioData;
        private AudioSource _musicSource;
        private IInstanceCreator _instanceCreator;
        private IResourceLoader _resourceLoader;
        private Transform _poolRoot;
        private int _currentPoolIndex = 0;
        private readonly List<AudioSource> _sfxPool = new List<AudioSource>();

        public void Initialize(AudioData audioData, IInstanceCreator instanceCreator, IResourceLoader resourceLoader, ushort poolSize)
        {
            _instanceCreator = instanceCreator;
            _resourceLoader = resourceLoader;
            _audioData = audioData;

            _poolRoot = new GameObject("[AudioPool]").transform;

            _musicSource = CreateAudioSource("Music_Source");
            _musicSource.loop = true;
            _musicSource.volume = _audioData.MusicVolume;

            FillUpAudioPool(poolSize);
        }

        public void PlayLaserShoot()
        {
            PlaySFX(_audioData.LaserShootClip);
        }

        public void PlayExplosion()
        {
            PlaySFX(_audioData.ExplosionClip);
        }

        public void PlayFireballShoot()
        {
            PlaySFX(_audioData.FireballShootClip);
        }

        public void PlayBackgroundMusic()
        {
            if (!_musicSource.isPlaying)
                _musicSource.Play();
        }

        public void SetMusicVolume(float volume)
        {
            if (_musicSource != null)
                _musicSource.volume = volume;
        }

        public void SetExplosionVolume(float volume)
        {
            foreach (var source in _sfxPool)
                source.volume = volume;
        }

        public void Dispose()
        {
            _sfxPool.Clear();

            if (_poolRoot != null)
            {
                Object.Destroy(_poolRoot.gameObject);
                _poolRoot = null;
            }

            _musicSource = null;
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

        private void FillUpAudioPool(ushort poolSize)
        {
            _sfxPool.Clear();

            for (int i = 0; i < poolSize; i++)
            {
                var source = CreateAudioSource($"SFX_Source_{i}");
                source.volume = _audioData.SfxVolume;
                _sfxPool.Add(source);
            }
        }

        private AudioSource CreateAudioSource(string name)
        {
            var sourceObj = _resourceLoader.Instantiate(_audioData.AudioSourcePrefab, _poolRoot);
            sourceObj.name = name;

            if (!sourceObj.TryGetComponent<AudioSource>(out AudioSource source))
            {
                source = sourceObj;
            }

            source.playOnAwake = false;
            return source;
        }
    }
}