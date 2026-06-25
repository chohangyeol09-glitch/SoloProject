using System;
using _02.Scripts.Agent;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.DeckSystem;
using UnityEngine;

namespace _02.Scripts.Players
{
    public class Player : AbstractAgent
    {
        public static Player Instance { get; private set; }
        
        [field: SerializeField] public int MaxCost { get; private set; } = 3;
        public int CurrentCost { get; private set; }
        private int _pendingCostPenalty;

        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private EventChannelSO turnChannel;
        [SerializeField] private StartStatCardListSO startStatCardListSO;
        public PlayerRuntimeDeck RuntimeDeck { get; private set; } = new();

        private void Awake()
        {
            Instance = this;
            CurrentHealth = MaxHealth;
            CurrentCost = MaxCost;
            RuntimeDeck.OnDeckCountChanged += count =>
                playerChannel.RaiseEvent(new DeckCountChangedEvent().Init(count));

            playerChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.AddListener<HealEvent>(HandleHeal);
            playerChannel.AddListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.AddListener<GainCostEvent>(HandleGainCost);
            playerChannel.AddListener<RecoverCostEvent>(HandleRecoverCost);
            playerChannel.AddListener<BurnCostEvent>(HandleBurnCost);
            turnChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void Start()
        {
            RuntimeDeck.Initialize(startStatCardListSO);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.RemoveListener<HealEvent>(HandleHeal);
            playerChannel.RemoveListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.RemoveListener<GainCostEvent>(HandleGainCost);
            playerChannel.RemoveListener<RecoverCostEvent>(HandleRecoverCost);
            playerChannel.RemoveListener<BurnCostEvent>(HandleBurnCost);
            turnChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void HandleTakeDamage(TakeDamageEvent evt) => TakeDamage(evt.Value);
        private void HandleHeal(HealEvent evt) => Heal(evt.Amount);

        private void HandleSpendCost(SpendCostEvent evt)
        {
            if (CurrentCost < evt.Amount) { evt.OnResult?.Invoke(false); return; }
            CurrentCost -= evt.Amount;
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
            evt.OnResult?.Invoke(true);
        }

        private void HandleGainCost(GainCostEvent evt)
        {
            CurrentCost = Mathf.Min(CurrentCost + evt.Amount, MaxCost);
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
        }

        private void HandleRecoverCost(RecoverCostEvent evt)
        {
            CurrentCost = evt.OverrideAmount ?? MaxCost;
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
        }

        private void HandleBurnCost(BurnCostEvent evt)
        {
            _pendingCostPenalty += Mathf.Max(0, evt.Amount);
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            int recovered = Mathf.Max(0, MaxCost - _pendingCostPenalty);
            _pendingCostPenalty = 0;
            playerChannel.RaiseEvent(new RecoverCostEvent().Init(recovered));
        }

        protected override void OnHealthChanged()
            => playerChannel.RaiseEvent(new HealthChangedEvent().Init(MaxHealth, CurrentHealth));

        protected override void OnDead()
        {
            playerChannel.RaiseEvent(new PlayerDeadEvent());
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}