using Zenject;
using UnityEngine;
using Asteroid.Audio;
using System.Collections.Generic;
using Asteroid.Generation;

namespace Asteroid.Installers
{
    public class AudioInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AudioSource>().FromMethod((context) => context.Container.Resolve<IInstanceCreator>().CreateInstance<GameObject>().AddComponent<AudioSource>()).AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        }
    }
}
