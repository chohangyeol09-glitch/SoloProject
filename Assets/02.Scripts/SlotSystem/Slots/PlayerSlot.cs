using System;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.InteractionSystem.Interactions;
using EPOOutline;
using UnityEngine;

namespace _02.Scripts.SlotSystem.Slots
{
    public class PlayerSlot : AbstractSlot
    {
        [SerializeField] private EventChannelSO slotEventChannel;

        public event Action<AbstractSlot, int> OnDropCard;
        public DropInteraction DropInteraction { get; private set; }

        private Outlinable _outline;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            _outline = GetComponent<Outlinable>();
            DropInteraction = GetModule<DropInteraction>();
            Debug.Assert(DropInteraction != null, "DropInteraction is null: " + gameObject.name);
            DropInteraction.SetCanDropType(typeof(ActionCard));
        }

        protected override void AfterInitializeModules()
        {
            DropInteraction.OnDrop += HandleDrop;
            DropInteraction.OnHoverEnter += HandleHoverEnter;
            DropInteraction.OnHoverExit += HandleHoverExit;
            slotEventChannel.AddListener<CardPickUpEvent>(HandleCardPickedUp);
        }

        private void OnDestroy()
        {
            DropInteraction.OnDrop -= HandleDrop;
            slotEventChannel.RemoveListener<CardPickUpEvent>(HandleCardPickedUp);
        }

        private void HandleCardPickedUp(CardPickUpEvent evt)    
        {
            if (CurrentCard != evt.Card) return;
            RemoveCurrentCard();
        }
        
        private void HandleHoverEnter()
        {
            _outline.OutlineParameters.Color = Color.white;
        }

        private void HandleHoverExit()
        {
            _outline.OutlineParameters.Color = Color.clear;
        }

        private void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<PlayerActionCard>(out PlayerActionCard incomingCard)) return;

            if (CurrentCard != null)
            {
                // 스왑 - 드래그한 카드의 원래 슬롯 찾기
                SwapCards(incomingCard);
                return;
            }

            SetCurrentCard(incomingCard);
            OnDropCard?.Invoke(this, SlotNumber);
        }
        
        private void SwapCards(PlayerActionCard incomingCard)
        {
            PlayerActionCard existingCard = CurrentCard as PlayerActionCard;
            PlayerSlot incomingOriginalSlot = incomingCard.OriginalSlot; // ← 먼저 저장

            SetCurrentCard(incomingCard); // ← 이제 덮어씌워져도 상관없음
            OnDropCard?.Invoke(this, SlotNumber);

            if (incomingOriginalSlot != null)
            {
                Debug.Log($"OriginalSlot: {incomingOriginalSlot}");
                incomingOriginalSlot.SetCurrentCard(existingCard);
                incomingOriginalSlot.OnDropCard?.Invoke(incomingOriginalSlot, incomingOriginalSlot.SlotNumber);
            }
            else
            {
                existingCard?.ForceReturn();
            }
        }
    }
}