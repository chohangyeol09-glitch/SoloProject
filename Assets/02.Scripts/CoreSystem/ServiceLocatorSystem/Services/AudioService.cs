using System;
using System.Collections.Generic;
using System.Linq;
using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using UnityEngine;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Services
{
    [Serializable]
    public struct NamedClip
    {
        public string name;
        public AudioClip clip;
    }

    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private NamedClip[] clips;

        private Dictionary<string, AudioClip> _clipMap;

        private void Awake()
        {
            _clipMap = clips.ToDictionary(clip => clip.name, clip => clip.clip);
            ServiceLocator.Register<IAudioService> (this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Register<IAudioService>(new NullAudioService()); 
        }

        public void PlaySFX(string clipName)
        {
            Debug.Log($"[AudioService] Playing {clipName}");
            if (_clipMap.TryGetValue(clipName, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip);
                Debug.Log($"[AudioService] Played {clipName}");
            }
        }

        public void PlayBGM(string clipName)
        {
            if (!_clipMap.TryGetValue(clipName, out AudioClip clip)) return;
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }
    }
}