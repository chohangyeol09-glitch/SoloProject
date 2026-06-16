using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using UnityEngine;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Services
{
    public class NullParticleService : IParticleService
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterDefault()
        {
            ServiceLocator.Register<IParticleService>(new NullParticleService());
        }
        
        public void PlayParticle(string particleName, Vector3 pos) { }
    }
}