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

        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private EventChannelSO turnChannel;
        [SerializeField] private StartStatCardListSO startStatCardListSO;
        public PlayerRuntimeDeck RuntimeDeck { get; private set; } = new();

        private void Awake()
        {
            Instance = this;
            CurrentHealth = MaxHealth;
            CurrentCost = MaxCost;
            RuntimeDeck.Initialize(startStatCardListSO);

            playerChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.AddListener<HealEvent>(HandleHeal);
            playerChannel.AddListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.AddListener<GainCostEvent>(HandleGainCost);
            playerChannel.AddListener<RecoverCostEvent>(HandleRecoverCost);
            turnChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
            
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            playerChannel.RemoveListener<HealEvent>(HandleHeal);
            playerChannel.RemoveListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.RemoveListener<GainCostEvent>(HandleGainCost);
            playerChannel.RemoveListener<RecoverCostEvent>(HandleRecoverCost);
            turnChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void HandleTakeDamage(TakeDamageEvent evt) => TakeDamage(evt.Value);
        private void HandleHeal(HealEvent evt) => Heal(evt.Amount);

        private void HandleSpendCost(SpendCostEvent evt)
        {
            if (CurrentCost < evt.Amount) { evt.OnResult?.Invoke(false); return; }
            Debug.Log($"HandleSpendCost {evt.Amount}");
            CurrentCost -= evt.Amount;
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
            evt.OnResult?.Invoke(true);
        }

        private void HandleGainCost(GainCostEvent evt)
        {
            Debug.Log($"HandleGainCost {evt.Amount}");
            CurrentCost = Mathf.Min(CurrentCost + evt.Amount, MaxCost);
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
        }

        private void HandleRecoverCost(RecoverCostEvent evt)
        {
            CurrentCost = evt.OverrideAmount ?? MaxCost;
            Debug.Log($"HandleRecoverCost {evt.OverrideAmount}");
            playerChannel.RaiseEvent(new CostChangedEvent().Init(CurrentCost));
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            playerChannel.RaiseEvent(new RecoverCostEvent().Init());
        }

        protected override void OnHealthChanged()
            => playerChannel.RaiseEvent(new HealthChangedEvent().Init(CurrentHealth));

        protected override void OnDead()
            => playerChannel.RaiseEvent(new PlayerDeadEvent());
    }
}