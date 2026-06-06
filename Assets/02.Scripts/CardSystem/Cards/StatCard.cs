using System;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.DeckSystem;
using _02.Scripts.UI;
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
        public StatCardUIChanger UIChanger { get; private set; }
        [field: SerializeField] public StatCardDataSO StatData { get; private set; }

        

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            UIChanger = GetModule<StatCardUIChanger>();
            CardInteraction.OnDragStart += HandleDragStart;
            CardInteraction.OnDragUpdate += HandleDragUpdate;
            CardInteraction.OnDragEnd += HandleDragEnd;
            CardInteraction.OnHoverEnter += HandleHoverEnter;
            CardInteraction.OnHoverExit += HandleHoverExit;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CardInteraction.OnDragStart -= HandleDragStart;
            CardInteraction.OnDragUpdate -= HandleDragUpdate;
            CardInteraction.OnDragEnd -= HandleDragEnd;
            CardInteraction.OnHoverEnter -= HandleHoverEnter;
            CardInteraction.OnHoverExit -= HandleHoverExit;
        }

        #region Handles

        // StatCard.cs
        protected override void HandleHoverEnter()
        {
            if (_handLogic == null) return;
            int index = _handLogic.GetCardIndex(this);
            int count = _handLogic.GetHandCount();
            Vector3 basePos = _handLogic.GetCardPosition(index, count);
            float targetY = (index + 2) * _handLogic.GetYPerIndex();
            basePos.y = targetY;

            transform.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(basePos, 0.15f));
            seq.Join(transform.DORotateQuaternion(_handLogic.GetCardRotation(index, count), 0.15f));
        }

        protected override void HandleHoverExit()
        {
            if (_handLogic == null) return;
            int index = _handLogic.GetCardIndex(this);
            int count = _handLogic.GetHandCount();

            transform.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(_handLogic.GetCardPosition(index, count), 0.15f));
            seq.Join(transform.DORotateQuaternion(_handLogic.GetCardRotation(index, count), 0.15f));
        }

        protected override void HandleDragEnd()
        {
            _handLogic?.ReturnCard(this);
        }
        
        #endregion
        
        public void SetHandManager(HandLogic handManager)
        {
            _handLogic = handManager;
        }
        public void ReturnToOrigin()
        {
            _handLogic?.ReturnCard(this);
        }

        public void SetStatData(StatCardDataSO statData)
        {
            StatData = statData;
            NeedCost = StatData.Cost;
            UIChanger.SetUI(StatData);
        }

        public void OnUsed(Action onComplete = null)
        {
            _handLogic?.RemoveCard(this);
            cardEventChannel.RaiseEvent(new DiscardCardEvent().Init(StatData)); 

            transform.DOKill();
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}