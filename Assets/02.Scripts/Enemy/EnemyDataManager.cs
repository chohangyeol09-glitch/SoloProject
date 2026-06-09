using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using UnityEngine;

namespace _02.Scripts.Enemy
{
    public class EnemyDataManager : MonoSingleton<EnemyDataManager>
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;
        public bool IsDead { get; private set; } = false;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private EventChannelSO gameEventChannel;
    
        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                enemyEventChannel?.RaiseEvent(new HealthChangedEvent().Init(_currentHealth));
                
                if (_currentHealth <= 0)
                    IsDead = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            IsDead = false;
            MaxHealth = evt.EnemyData.MaxHealth;
            CurrentHealth = MaxHealth;
        }
    }
}