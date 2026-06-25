using System;
using System.Collections.Generic;
using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using UnityEngine;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Services
{
    public class ParticleService : MonoBehaviour, IParticleService
    {

        [Serializable]
        public struct NamedParticle
        {
            public string Name;
            public GameObject Particle;
        }

        [SerializeField] private List<NamedParticle> particles;

        private Dictionary<string, GameObject> _particleMap;

        private void Awake()
        {
            _particleMap = new Dictionary<string, GameObject>();
            foreach (NamedParticle particle in particles)
            {
                if (particle.Particle == null || string.IsNullOrEmpty(particle.Name)) continue;
                _particleMap[particle.Name] = particle.Particle;
            }

            ServiceLocator.Register<IParticleService>(this);
        }

        public void PlayParticle(string particleName, Vector3 pos)
        {
            if (string.IsNullOrEmpty(particleName)) return;

            if (_particleMap == null || !_particleMap.TryGetValue(particleName, out GameObject prefab) || prefab == null)
            {
                Debug.LogWarning($"[ParticleService] Particle '{particleName}' is not registered.");
                return;
            }

            GameObject instance = Instantiate(prefab, pos, Quaternion.identity);

            float lifetime = 5f;
            if (instance.TryGetComponent(out ParticleSystem ps))
                lifetime = ps.main.duration + ps.main.startLifetime.constantMax;

            Destroy(instance, lifetime);
        }
    }
}
