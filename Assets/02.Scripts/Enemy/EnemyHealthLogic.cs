using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvent;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.Enemy
{
    public class EnemyHealthLogic : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO enemyChannel;

        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        private ModuleOwner _owner;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            enemyChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyChannel.AddListener<HealEvent>(HandleHeal);
        }

        private void OnDestroy()
        {
            enemyChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyChannel.RemoveListener<HealEvent>(HandleHeal);
        }

        private void HandleTakeDamage(TakeDamageEvent evt)
        {
            EnemyDataManager.Instance.CurrentHealth -= evt.Amount;
            EnemyDataManager.Instance.CurrentHealth = Mathf.Max(
                EnemyDataManager.Instance.CurrentHealth, 0
            );
            OnHealthChanged?.Invoke(EnemyDataManager.Instance.CurrentHealth);

            bool isDead = EnemyDataManager.Instance.CurrentHealth <= 0;
            if (isDead)
            {
                OnDead?.Invoke();
                enemyChannel.RaiseEvent(new EnemyDeadEvent());
            }
        }

        private void HandleHeal(HealEvent evt)
        {
            EnemyDataManager.Instance.CurrentHealth += evt.Amount;
            EnemyDataManager.Instance.CurrentHealth = Mathf.Min(
                EnemyDataManager.Instance.CurrentHealth,
                EnemyDataManager.Instance.MaxHealth
            );
            OnHealthChanged?.Invoke(EnemyDataManager.Instance.CurrentHealth);
        }
        
        
    }
}