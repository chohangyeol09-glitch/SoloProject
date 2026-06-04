using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StatCardEvent;
using _02.Scripts.DeckSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards
{
    public class StatCard : AbstractCard
    {
        [SerializeField] private EventChannelSO cardEventChannel;

        private HandLogic _handLogic;
        private int _handIndex = -1;
        private Vector3 _handPos;
        private Quaternion _handRot;
        private bool _isDragging = false;
        
        public int NeedCost { get; private set; }
        [field: SerializeField] public StatCardDataSO StatData { get; private set; }

        public void SetHandManager(HandLogic handManager)
        {
            _handLogic = handManager;
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            CardInteraction.OnDragStarted += HandleDragStarted;
            CardInteraction.OnDragUpdated += HandleDragUpdated;
            CardInteraction.OnDragEnded += HandleDragEnded;
            CardInteraction.OnHoverEntered += HandleHoverEntered;
            CardInteraction.OnHoverExited += HandleHoverExited;
        }

        #region Handles

        private void HandleDragStarted()
        {
            if (_handLogic != null)
            {
                _handIndex = _handLogic.GetCardIndex(this);
                _handPos = _handLogic.GetCardPosition(_handIndex, _handLogic.GetHandCount());
                _handRot = _handLogic.GetCardRotation(_handIndex, _handLogic.GetHandCount());
            }
            transform.DOKill();
            transform.DOMove(transform.position + Vector3.up * 1.5f, 0.15f);
        }

        private void HandleDragUpdated(Vector3 pos)
        {
            transform.DOKill();
            transform.position = pos;
        }

        private void HandleDragEnded()
        {
            _handLogic?.ReturnCard(this);
        }

        private void HandleHoverEntered()
        {
            if (_handLogic == null) return;
            int index = _handLogic.GetCardIndex(this);
            int count = _handLogic.GetHandCount();
            Vector3 basePos = _handLogic.GetCardPosition(index, count);
            float targetY = (index + 2) * _handLogic.GetYPerIndex();
            basePos.y = targetY;
            transform.DOKill();
            transform.DOMove(basePos, 0.15f);
        }

        private void HandleHoverExited()
        {
            if (_handLogic == null) return;
            int index = _handLogic.GetCardIndex(this);
            int count = _handLogic.GetHandCount();
            transform.DOKill();
            transform.DOMove(_handLogic.GetCardPosition(index, count), 0.15f);
        }
        
        #endregion
        
        public void ReturnToOrigin()
        {
            _handLogic?.ReturnCard(this);
        }

        public void SetStatData(StatCardDataSO statData)
        {
            StatData = statData;
            NeedCost = StatData.Cost;
        }

        public void OnUsed()
        {
            _handLogic?.RemoveCard(this);
            cardEventChannel.RaiseEvent(new DiscardCardEvent().Init(StatData));
            Destroy(gameObject);
        }
    }
}