using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using UnityEngine;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Services
{
    public class NullAudioService : IAudioService
    {
        public void PlaySFX(string clipName) {}
        public void PlayBGM(string clipName) {}
        public void StopBGM() {}

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterDefault()
        {
            ServiceLocator.Register<IAudioService>(new NullAudioService()); 
        }
    }
}