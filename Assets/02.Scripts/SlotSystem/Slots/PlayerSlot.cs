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
            _outline.FrontParameters.Color = Color.white;
        }

        private void HandleHoverExit()
        {
            _outline.FrontParameters.Color = Color.clear;
        }

        private void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<PlayerActionCard>(out PlayerActionCard incomingCard)) return;

            if (CurrentCard != null)
            {
                if (incomingCard.OriginalSlot != null)
                {
                    SwapCards(incomingCard);
                }
                else
                {
                    incomingCard.ForceReturn();
                }
                return;
            }

            SetCurrentCard(incomingCard);
            OnDropCard?.Invoke(this, SlotNumber);
        }
        
        private void SwapCards(PlayerActionCard incomingCard)
        {
            PlayerActionCard existingCard = CurrentCard as PlayerActionCard;
            PlayerSlot incomingOriginalSlot = incomingCard.OriginalSlot;

            SetCurrentCard(incomingCard); 
            OnDropCard?.Invoke(this, SlotNumber);

            if (incomingOriginalSlot != null)
            {
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