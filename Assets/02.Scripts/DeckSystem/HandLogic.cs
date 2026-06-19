using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.DeckSystem
{
    public class HandLogic : MonoBehaviour, IModule , IAfterInitializeModule
    {
        [SerializeField] private EventChannelSO cardEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private GameObject statCardPrefab;
        [SerializeField] private Transform handCenter;
        [SerializeField] private int maxHandSize = 10;
        [SerializeField] private float cardSpacing = 1.5f; // 카드 간격
        [SerializeField] private float yPerIndex = 0.05f; // 인덱스마다 Y 증가
        [SerializeField] private float yHoverAddValue = 0.3f;
        [SerializeField] private float dragYOffset = 1.5f; // 드래그 시 Y
        [SerializeField] private float fanAngle = 30f; // 부채꼴 각도
        [SerializeField] private float centerZOffset = 0.1f;
        [SerializeField] private Vector3 cardDefaultRotation = new Vector3(-90, 0, 0);
        [SerializeField] private float arrangeDuration = 0.3f;
        public int DefaultDrawCount { get; set; } = 3;

        private DeckLogic _deckLogic;
        private List<StatCard> _handCards = new();

        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            cardEventChannel.AddListener<DrawCardEvent>(HandleDrawHand);
            turnEventChannel.AddListener<TurnChangeEvent>(HandleTurnStart);
            turnEventChannel.AddListener<ClearHandEvent>(HandleClearHand); 
        }
        
        public void AfterInitialize()
        {
            _deckLogic = _owner.GetComponent<GameManager>().DeckLogic;
        }
        
        private void OnDestroy()
        {
            cardEventChannel.RemoveListener<DrawCardEvent>(HandleDrawHand);
            turnEventChannel.RemoveListener<TurnChangeEvent>(HandleTurnStart);
            turnEventChannel.RemoveListener<ClearHandEvent>(HandleClearHand);
        }

        private void HandleDrawHand(DrawCardEvent evt)
        {
            DrawHand(evt.Count);
        }
        
        private void HandleTurnStart(TurnChangeEvent evt)
        {
            DrawHand(DefaultDrawCount);
        }
        
        private void HandleTurnEnd(TurnEndEvent evt)
        {
            ClearHand(); 
        }
        
        private void HandleClearHand(ClearHandEvent evt)
        {
            ClearHand(evt.OnComplete);
        }

        
        public void DrawHand(int count)
        {
            int canDraw = Mathf.Min(count, maxHandSize - _handCards.Count);
            if (canDraw <= 0) return;

            List<StatCardDataSO> drawn = _deckLogic.DrawCards(canDraw);
            foreach (StatCardDataSO data in drawn)
                AddCard(data);
        }

        public void ClearHand(Action onComplete = null)
        {
            if (_handCards.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            int remaining = _handCards.Count;
            List<StatCard> cardsToRemove = new List<StatCard>(_handCards);

            foreach (StatCard card in cardsToRemove)
            {
                if (card == null)
                {
                    remaining--;
                    if (remaining <= 0) 
                        onComplete?.Invoke();
                    continue;
                }
                card.OnUsed(() =>
                {
                    remaining--;
                    if (remaining <= 0)
                        onComplete?.Invoke();
                });
            }
        }

        public void RemoveCard(StatCard card)
        {
            if (!_handCards.Contains(card)) return;
            _handCards.Remove(card);
            ArrangeCards(); 
        }

        public void ReturnCard(StatCard card)
        {
            int index = _handCards.IndexOf(card);
            if (index < 0) return;
            Vector3 originPos = GetCardPosition(index, _handCards.Count);
            Quaternion originRot = GetCardRotation(index, _handCards.Count);
            card.transform.DOMove(originPos, arrangeDuration);
            card.transform.DORotateQuaternion(originRot, arrangeDuration);
        }

        private void AddCard(StatCardDataSO data)
        {
            if (_handCards.Count >= maxHandSize) return;

            GameObject obj = Instantiate(statCardPrefab);
            StatCard card = obj.GetComponent<StatCard>();
            card.SetStatData(data);
            card.SetHandManager(this);
            _handCards.Add(card);

            int newIndex = _handCards.Count - 1;
            int total = _handCards.Count;

            ArrangeCards();
        }

        public void ArrangeCards()
        {
            int count = _handCards.Count;
            for (int i = 0; i < count; i++)
            {
                _handCards[i].transform.DOMove(GetCardPosition(i, count), arrangeDuration); 
                _handCards[i].transform.DORotateQuaternion(GetCardRotation(i, count), arrangeDuration);
            }
        }

        public Vector3 GetCardPosition(int index, int total)
        {
            float t = total == 1 ? 0.5f : (float)index / (total - 1);
            float angle = Mathf.Lerp(-fanAngle / 2f, fanAngle / 2f, t);
            float rad = angle * Mathf.Deg2Rad;

            float centerDist = Mathf.Abs(index - (total - 1) * 0.5f);
            float zValue = -centerDist * centerZOffset; 

            return handCenter.position + new Vector3(
                Mathf.Sin(rad) * cardSpacing * total * 0.5f,
                index * yPerIndex,
                zValue
            );
        }

        public Quaternion GetCardRotation(int index, int total)
        {
            float t = total == 1 ? 0.5f : (float)index / (total - 1);
            float angle = Mathf.Lerp(-fanAngle / 2f, fanAngle / 2f, t);

            return Quaternion.Euler(0, angle, 0) * Quaternion.Euler(cardDefaultRotation);
        }
        
        public float GetYHoverAddValue() => yHoverAddValue;
        public int GetCardIndex(StatCard card) => _handCards.IndexOf(card);
        public int GetHandCount() => _handCards.Count;

#if UNITY_EDITOR
        [ContextMenu("TestDrawHand")]
        public void TestDrawHand() => DrawHand(DefaultDrawCount);

        [ContextMenu("TestClearHand")]
        public void TestClearHand() => ClearHand();
#endif


    }
}