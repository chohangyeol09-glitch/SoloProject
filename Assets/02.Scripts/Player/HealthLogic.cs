using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.PlayerEvent;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class HealthLogic : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO playerChannel;

        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            playerChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.AddListener<HealEvent>(HandleHeal);
        }
        
        private void OnDestroy()
        {
            playerChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.RemoveListener<HealEvent>(HandleHeal);
        }

        private void HandleTakeDamage(TakeDamageEvent evt)
        {
            PlayerDataManager.Instance.CurrentHealth -= evt.Amount;
            PlayerDataManager.Instance.CurrentHealth = Mathf.Max(
                PlayerDataManager.Instance.CurrentHealth, 0
            );
            OnHealthChanged?.Invoke(PlayerDataManager.Instance.CurrentHealth);

            bool isDead = PlayerDataManager.Instance.CurrentHealth <= 0;
            evt.OnDead?.Invoke(isDead); 
            if (isDead) OnDead?.Invoke();
        }

        private void HandleHeal(HealEvent evt)
        {
            PlayerDataManager.Instance.CurrentHealth += evt.Amount;
            PlayerDataManager.Instance.CurrentHealth = Mathf.Min(
                PlayerDataManager.Instance.CurrentHealth,
                PlayerDataManager.Instance.MaxHealth
            );
            OnHealthChanged?.Invoke(PlayerDataManager.Instance.CurrentHealth);
        }


    }
}