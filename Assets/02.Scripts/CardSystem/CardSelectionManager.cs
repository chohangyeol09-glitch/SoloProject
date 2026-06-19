using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem;
using _02.Scripts.Players;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    public class CardSelectionManager : MonoSingleton<CardSelectionManager>
    {
        [Header("Input")]
        [SerializeField] private PlayerInputSO playerInput;

        [Header("Pool")]
        [SerializeField] private CardGradePoolSO gradePool;

        [Header("Prefabs")]
        [SerializeField] private GameObject playerActionCardPrefab;
        [SerializeField] private GameObject actionEffectCardPrefab;
        [SerializeField] private GameObject statCardPrefab;

        [Header("Selection")]
        [SerializeField] private int selectionCount = 3;
        [SerializeField] private List<AbstractSlot> actionCardStorage = new();

        [Header("Common")]
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private Transform createPoint;
        [SerializeField] private Transform sortCenter;
        [SerializeField] private float cardSpacing = 2f;

        public event Action OnGameStartSelectionComplete;

        private readonly List<AbstractCard> _selectionCards = new();
        private bool _isSelecting;
        private bool _roundComplete;
        private Vector2 _mousePos;

        private static readonly CardGrade[] GameStartSequence =
        {
            CardGrade.BRONZE, CardGrade.BRONZE, CardGrade.SILVER, CardGrade.GOLD
        };

        private void Awake()
        {
            playerInput.OnMovePointer += OnMouseMove;
            playerInput.OnClickDown += OnClickDown;
        }

        private void OnDestroy()
        {
            playerInput.OnMovePointer -= OnMouseMove;
            playerInput.OnClickDown -= OnClickDown;
        }

        public async UniTask ShowRewards(List<RewardEntry> rewards)
        {
            if (rewards == null) return;

            foreach (RewardEntry entry in rewards)
            {
                _roundComplete = false;
                ShowSelection(entry.Type, entry.Grade);
                await UniTask.WaitUntil(() => _roundComplete);
            }
        }

#region StartSeq
        private void StartGameCardSelection() => StartCoroutine(GameStartSequenceCoroutine());

        private IEnumerator GameStartSequenceCoroutine()
        {
            using (PresentationControl.Busy())
            {
                foreach (CardGrade grade in GameStartSequence)
                {
                    _roundComplete = false;
                    ShowSelection(CardPoolType.Action, grade);
                    yield return new WaitUntil(() => _roundComplete);
                }
            }

            OnGameStartSelectionComplete?.Invoke();
        }
#endregion

        private void ShowSelection(CardPoolType type, CardGrade grade)
        {
            List<(CardPoolType type, ScriptableObject data)> picks = PickCards(type, grade, selectionCount);
            SpawnCards(picks);
            _isSelecting = true;
            ChangeCamera.Instance?.SetTopView();
        }

        private void OnMouseMove(Vector2 pos) => _mousePos = pos;

        private void OnClickDown()
        {
            if (!_isSelecting) return;
            if (Camera.main == null) return;

            Ray ray = Camera.main.ScreenPointToRay(_mousePos);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cardLayer)) return;

            AbstractCard selected = hit.collider.GetComponent<AbstractCard>();
            if (selected == null || !_selectionCards.Contains(selected)) return;

            _isSelecting = false;
            HandleCardSelected(selected);
        }

        private void HandleCardSelected(AbstractCard selected)
        {
            foreach (AbstractCard card in _selectionCards)
            {
                if (card == selected) continue;
                Destroy(card.gameObject);
            }
            _selectionCards.Clear();

            if (selected is PlayerActionCard actionCard)
            {
                GetNextEmptyStorageSlot()?.SetCurrentCard(actionCard);
            }
            else if (selected is ActionEffectCard)
            {
                
            }
            else if (selected is StatCard statCard)
            {
                Player.Instance.RuntimeDeck.AddCard(statCard.StatData);
                Destroy(selected.gameObject);
            }

            CompleteSelection();
        }

        private void CompleteSelection()
        {
            ChangeCamera.Instance?.RestoreView();
            _roundComplete = true;
        }

        public async UniTask CollectCardsFromPlayerSlots(IEnumerable<AbstractSlot> playerSlots)
        {
            List<UniTask> drops = new();
            foreach (AbstractSlot slot in playerSlots)
            {
                if (slot.CurrentCard == null) continue;
                ActionCard card = slot.CurrentCard;
                slot.RemoveCurrentCard();
                Tween drop = GetNextEmptyStorageSlot()?.SetCurrentCard(card);
                if (drop != null) drops.Add(drop.ToUniTask());
            }

            if (drops.Count > 0) await UniTask.WhenAll(drops);
        }

        private AbstractSlot GetNextEmptyStorageSlot()
        {
            foreach (AbstractSlot slot in actionCardStorage)
                if (slot.CurrentCard == null)
                    return slot;
            return null;
        }

        private List<(CardPoolType type, ScriptableObject data)> PickCards(CardPoolType type, CardGrade grade, int count)
        {
            var all = new List<(CardPoolType type, ScriptableObject data)>();

            switch (type)
            {
                case CardPoolType.Action:
                    foreach (ActionCardDataSO d in gradePool.GetActionCardsByGrade(grade))
                        all.Add((CardPoolType.Action, d));
                    break;
                case CardPoolType.ActionEffect:
                    foreach (AbstractActionEffectSO d in gradePool.GetEffectsByGrade(grade))
                        all.Add((CardPoolType.ActionEffect, d));
                    break;
                case CardPoolType.Stat:
                    foreach (StatCardDataSO d in gradePool.GetStatCardsByGrade(grade))
                        all.Add((CardPoolType.Stat, d));
                    break;
            }

            Shuffle(all);
            return all.Count <= count ? all : all.GetRange(0, count);
        }

        private void SpawnCards(List<(CardPoolType type, ScriptableObject data)> picks)
        {
            int total = picks.Count;
            for (int i = 0; i < total; i++)
            {
                float x = (i - (total - 1) * 0.5f) * cardSpacing;
                Vector3 pos = createPoint.transform.position;
                AbstractCard card = SpawnCard(picks[i].type, picks[i].data, pos);
                card.transform.DOMove(sortCenter.position + new Vector3(x, 0f, 0f), 0.4f).SetEase(Ease.OutQuint);
                if (card != null)
                    _selectionCards.Add(card);
            }
        }

        private AbstractCard SpawnCard(CardPoolType type, ScriptableObject data, Vector3 pos)
        {
            GameObject prefab = type switch
            {
                CardPoolType.Action       => playerActionCardPrefab,
                CardPoolType.ActionEffect => actionEffectCardPrefab,
                CardPoolType.Stat         => statCardPrefab,
                _                         => null
            };
            if (prefab == null) return null;

            GameObject obj = Instantiate(prefab, pos, sortCenter.rotation);

            switch (type)
            {
                case CardPoolType.Action:
                    var actionCard = obj.GetComponent<PlayerActionCard>();
                    actionCard.SetActionCardData(data as ActionCardDataSO);
                    return actionCard;

                case CardPoolType.ActionEffect:
                    var effectCard = obj.GetComponent<ActionEffectCard>();
                    effectCard.SetEffect(data as AbstractActionEffectSO);
                    return effectCard;

                case CardPoolType.Stat:
                    var statCard = obj.GetComponent<StatCard>();
                    statCard.SetStatData(data as StatCardDataSO);
                    return statCard;
            }
            return null;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Test GameStart Selection")]
        private void TestGameStart() => StartGameCardSelection();
#endif
    }
}
