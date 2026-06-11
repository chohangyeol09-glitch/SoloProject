using _02.Scripts.Agent;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    public class Enemy : AbstractAgent
    {
        public static Enemy Instance { get; private set; }

        [SerializeField] private EventChannelSO enemyChannel;
        [SerializeField] private EventChannelSO gameEventChannel;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            Instance = this;
            enemyChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyChannel.AddListener<HealEvent>(HandleHeal);
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
        }

        private void OnDestroy()
        {
            enemyChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyChannel.RemoveListener<HealEvent>(HandleHeal);
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            IsDead = false;
            MaxHealth = evt.EnemyData.MaxHealth;
            CurrentHealth = MaxHealth;
            OnHealthChanged();
        }

        private void HandleTakeDamage(TakeDamageEvent evt) => TakeDamage(evt.Value);
        private void HandleHeal(HealEvent evt) => Heal(evt.Amount);

        protected override void OnHealthChanged()
            => enemyChannel.RaiseEvent(new HealthChangedEvent().Init(CurrentHealth));

        protected override void OnDead()
            => IsDead = true;
    }
}