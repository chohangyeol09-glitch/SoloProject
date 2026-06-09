using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.DackSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class PlayerDataManager : MonoSingleton<PlayerDataManager>
    {
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

        public PlayerRuntimeDeck RuntimeDeck { get; private set; } = new();

        [SerializeField] private EventChannelSO playerEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private StartStatCardListSO startStatCardListSO;
        [field: SerializeField] public int MaxCost { get; private set; } = 3;
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;

        
        private int _currentCost;
        private int _currentHealth;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            RuntimeDeck.Initialize(startStatCardListSO);
            turnEventChannel.AddListener<TurnChangeEvent>(HandleTurnStart);
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnDestroy()
        {
            turnEventChannel.RemoveListener<TurnChangeEvent>(HandleTurnStart);
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + MaxHealth / 10, MaxHealth);
        }

        private void HandleTurnStart(TurnChangeEvent evt)
        {
            playerEventChannel.RaiseEvent(new RecoverCostEvent().Init());
        }
    }
}