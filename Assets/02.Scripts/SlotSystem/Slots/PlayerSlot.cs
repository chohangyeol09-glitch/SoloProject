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
        [SerializeField] private Color sealColor = Color.red;

        public event Action<AbstractSlot, int> OnDropCard;
        public DropInteraction DropInteraction { get; private set; }
        public bool IsSealed { get; private set; }

        private Outlinable _outline;
        private Func<bool> _trySwap;

        // 교환 횟수 제한을 검사/소비하는 함수를 주입한다(SlotLogic이 턴당 예산을 관리).
        public void SetSwapPermission(Func<bool> trySwap) => _trySwap = trySwap;
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

        public void Seal()
        {
            IsSealed = true;
            if (_outline != null)
                _outline.FrontParameters.Color = sealColor;
        }

        public void Unseal()
        {
            IsSealed = false;
            if (_outline != null)
                _outline.FrontParameters.Color = Color.clear;
        }
        
        private void HandleHoverEnter()
        {
            if (IsSealed) return;
            _outline.FrontParameters.Color = Color.white;
        }

        private void HandleHoverExit()
        {
            if (IsSealed) return;
            _outline.FrontParameters.Color = Color.clear;
        }

        private void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<PlayerActionCard>(out PlayerActionCard incomingCard)) return;

            if (IsSealed)
            {
                incomingCard.ForceReturn();
                return;
            }

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
            // 턴당 교환 횟수를 초과하면 교환을 막고 끌어온 카드를 원래 자리로 되돌린다.
            if (_trySwap != null && !_trySwap())
            {
                incomingCard.ForceReturn();
                return;
            }

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