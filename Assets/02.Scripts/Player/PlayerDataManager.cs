using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.DackSystem;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class PlayerDataManager : MonoSingleton<PlayerDataManager>
    {
        [SerializeField] private EventChannelSO playerEventChannel;

        [field: SerializeField] public int MaxCost { get; private set; } = 3;
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;

        private int _currentCost;
        private int _currentHealth;

        public int CurrentCost
        {
            get => _currentCost;
            set
            {
                _currentCost = value;
                playerEventChannel?.RaiseEvent(new CostChangedEvent().Init(_currentCost));
            }
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                playerEventChannel?.RaiseEvent(new HealthChangedEvent().Init(_currentHealth));
            }
        }

        [SerializeField] private StartStatCardListSO startStatCardListSO;
        public PlayerRuntimeDeck RuntimeDeck { get; private set; } = new();

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            RuntimeDeck.Initialize(startStatCardListSO);
            CurrentCost = MaxCost;
            CurrentHealth = MaxHealth;
        }
    }
}