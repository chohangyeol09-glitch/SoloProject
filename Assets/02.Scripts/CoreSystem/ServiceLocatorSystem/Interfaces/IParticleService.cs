using UnityEngine;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces
{
    public interface IParticleService
    {
        void PlayParticle(string particleName, Vector3 pos);
    }
}