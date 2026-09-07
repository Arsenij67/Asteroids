using UnityEngine;

namespace Asteroid.Audio
{

    [CreateAssetMenu(fileName = "AudioData", menuName = "Game/Audio Data")]
    public class AudioData : ScriptableObject
    {
        public AudioClip ShootClip;
        public AudioClip ExplosionClip;
        public AudioClip BackgroundMusic;

        [Range(0f, 1f)] public float SfxVolume = 1f;
        [Range(0f, 1f)] public float MusicVolume = 0.5f;
    }
}
