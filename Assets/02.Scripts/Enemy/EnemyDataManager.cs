using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using UnityEngine;

namespace _02.Scripts.Enemy
{
    public class EnemyDataManager : MonoSingleton<EnemyDataManager>
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;
        [SerializeField] private EventChannelSO enemyEventChannel;

        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                enemyEventChannel?.RaiseEvent(new HealthChangedEvent().Init(_currentHealth));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }
        
        public void SetMaxHealth(int value)
        {
            MaxHealth = value;
            CurrentHealth = value;
        }
    }
}