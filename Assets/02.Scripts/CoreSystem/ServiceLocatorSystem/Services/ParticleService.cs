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
        
        public void PlayParticle(string particleName, Vector3 pos)
        {
            _particleMap = new();

            foreach (NamedParticle particle in particles)
            {
                _particleMap[particle.Name] = particle.Particle;
            }
        }
        
        
    }
}