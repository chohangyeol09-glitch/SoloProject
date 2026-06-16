using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.Players;
using _02.Scripts.SlotSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    public class CardSelectionManager : MonoBehaviour
    {
        [Header("Channels")]
        [SerializeField] private EventChannelSO stageEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private PlayerInputSO playerInput;

        [Header("Pool")]
        [SerializeField] private CardGradePoolSO gradePool;

        [Header("Prefabs")]
        [SerializeField] private GameObject playerActionCardPrefab;
        [SerializeField] private GameObject actionEffectCardPrefab;
        [SerializeField] private GameObject statCardPrefab;

        [Header("Stage Clear Selection")]
        [SerializeField] private PoolType stageClearRewardType = PoolType.Action;
        [SerializeField] private CardGrade stageClearRewardGrade = CardGrade.BRONZE;
        [SerializeField] private int stageClearRewardCount = 3;

        [Header("Game Start Selection")]
        [SerializeField] private int gameStartPoolSize = 3;
        [SerializeField] private List<AbstractSlot> actionCardStorage = new();

        [Header("Common")] 
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float cardSpacing = 2f;

        public event Action OnStageClearSelectionComplete;
        public event Action OnGameStartSelectionComplete;

        private readonly List<AbstractCard> _selectionCards = new();
        private bool _isSelecting;
        private bool _roundComplete;
        private bool _isGameStartMode;
        private Vector2 _mousePos;

        private static readonly CardGrade[] GameStartSequence =
        {
            CardGrade.BRONZE, CardGrade.BRONZE, CardGrade.SILVER, CardGrade.GOLD
        };

        private void Awake()
        {
            stageEventChannel.AddListener<StageClearEvent>(HandleStageClear);
            playerInput.OnMovePointer += OnMouseMove;
            playerInput.OnClickDown += OnClickDown;
        }

        private void OnDestroy()
        {
            stageEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
            playerInput.OnMovePointer -= OnMouseMove;
            playerInput.OnClickDown -= OnClickDown;
        }

        private void HandleStageClear(StageClearEvent evt) => ShowStageClearSelection();

        private void ShowStageClearSelection()
        {
            _isGameStartMode = false;
            turnEventChannel.RaiseEvent(new InteractionDisableEvent());

            List<(PoolType type, ScriptableObject data)> picks = PickCards(stageClearRewardType, stageClearRewardGrade, stageClearRewardCount);
            SpawnCards(picks);
            _isSelecting = true;
        }

        private void StartGameCardSelection()
        {
            _isGameStartMode = true;
            StartCoroutine(GameStartSequenceCoroutine());
        }

        private IEnumerator GameStartSequenceCoroutine()
        {
            foreach (CardGrade grade in GameStartSequence)
            {
                turnEventChannel.RaiseEvent(new InteractionDisableEvent());
                _roundComplete = false;

                ShowActionCardSelection(grade);

                yield return new WaitUntil(() => _roundComplete);
            }

            turnEventChannel.RaiseEvent(new InteractionEnableEvent());
            OnGameStartSelectionComplete?.Invoke();
        }

        private void ShowActionCardSelection(CardGrade grade)
        {
            var pool = new List<ActionCardDataSO>(gradePool.GetActionCardsByGrade(grade));
            Shuffle(pool);

            int count = Mathf.Min(gameStartPoolSize, pool.Count);
            var picks = new List<(PoolType type, ScriptableObject data)>();
            for (int i = 0; i < count; i++)
                picks.Add((PoolType.Action, pool[i]));

            SpawnCards(picks);
            _isSelecting = true;
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
            if (_isGameStartMode)
            {
                _roundComplete = true;
            }
            else
            {
                turnEventChannel.RaiseEvent(new InteractionEnableEvent());
                OnStageClearSelectionComplete?.Invoke();
            }
        }

        private AbstractSlot GetNextEmptyStorageSlot()
        {
            foreach (AbstractSlot slot in actionCardStorage)
                if (slot.CurrentCard == null)
                    return slot;
            return null;
        }


        private List<(PoolType type, ScriptableObject data)> PickCards(PoolType type, CardGrade grade, int count)
        {
            var all = new List<(PoolType type, ScriptableObject data)>();

            switch (type)
            {
                case PoolType.Action:
                    foreach (ActionCardDataSO d in gradePool.GetActionCardsByGrade(grade))
                        all.Add((PoolType.Action, d));
                    break;
                case PoolType.ActionEffect:
                    foreach (AbstractActionEffectSO d in gradePool.GetEffectsByGrade(grade))
                        all.Add((PoolType.ActionEffect, d));
                    break;
                case PoolType.Stat:
                    foreach (StatCardDataSO d in gradePool.GetStatCardsByGrade(grade))
                        all.Add((PoolType.Stat, d));
                    break;
            }

            Shuffle(all);
            return all.Count <= count ? all : all.GetRange(0, count);
        }

        private void SpawnCards(List<(PoolType type, ScriptableObject data)> picks)
        {
            int total = picks.Count;
            for (int i = 0; i < total; i++)
            {
                float x = (i - (total - 1) * 0.5f) * cardSpacing;
                Vector3 pos = spawnCenter.position + new Vector3(x, 0f, 0f);
                AbstractCard card = SpawnCard(picks[i].type, picks[i].data, pos);
                if (card != null)
                    _selectionCards.Add(card);
            }
        }

        private AbstractCard SpawnCard(PoolType type, ScriptableObject data, Vector3 pos)
        {
            GameObject prefab = type switch
            {
                PoolType.Action       => playerActionCardPrefab,
                PoolType.ActionEffect => actionEffectCardPrefab,
                PoolType.Stat         => statCardPrefab,
                _                    => null
            };
            if (prefab == null) return null;

            GameObject obj = Instantiate(prefab, pos, spawnCenter.rotation);

            switch (type)
            {
                case PoolType.Action:
                    var actionCard = obj.GetComponent<PlayerActionCard>();
                    actionCard.SetActionCardData(data as ActionCardDataSO);
                    return actionCard;

                case PoolType.ActionEffect:
                    var effectCard = obj.GetComponent<ActionEffectCard>();
                    effectCard.SetEffect(data as AbstractActionEffectSO);
                    return effectCard;

                case PoolType.Stat:
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

        private enum PoolType { Action, ActionEffect, Stat }

#if UNITY_EDITOR
        [ContextMenu("Test StageClear Selection")]
        private void TestStageClear() => ShowStageClearSelection();

        [ContextMenu("Test GameStart Selection")]
        private void TestGameStart() => StartGameCardSelection();
#endif
    }
}
