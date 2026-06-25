using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.Enemys;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.SlotSystem
{
    public class SlotLogic : MonoBehaviour, IModule
    {
        [SerializeField] private List<PlayerSlot> playerSlots = new();
        [SerializeField] private List<EnemySlot> enemySlots = new();
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private GameObject enemyActionCardPrefab;
        [SerializeField] private int maxSwapsPerTurn = 2;

        public IReadOnlyList<PlayerSlot> PlayerSlots => playerSlots;
        public IReadOnlyList<EnemySlot> EnemySlots => enemySlots;

        private List<GameObject> _spawnedCards = new();
        private ModuleOwner _owner;

        private EnemyDataSO _currentData;
        private int _remainingRespawns;
        private int _remainingSwaps;
        private readonly List<int> _pendingRespawns = new();
        private readonly Dictionary<int, int> _sealedSlots = new();

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;

            foreach (PlayerSlot slot in playerSlots)
            {
                slot.OnDropCard += UpdateSlot;
                slot.SetSwapPermission(TryConsumeSwap);
            }

            ResetSwaps();

            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);

            enemyChannel.AddListener<EnemyCardDestroyedEvent>(HandleEnemyCardDestroyed);
            turnEventChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);

            enemyChannel.RemoveListener<EnemyCardDestroyedEvent>(HandleEnemyCardDestroyed);
            turnEventChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            ClearEnemyCards();

            _currentData = evt.EnemyData;
            _remainingRespawns = evt.EnemyData.MaxRespawnCount;
            ResetSwaps();

            foreach (EnemyCardPlacement placement in evt.EnemyData.CardPlacements)
            {
                if (placement.SlotIndex < 0 || placement.SlotIndex >= enemySlots.Count) continue;
                if (placement.CardData == null) continue;

                GameObject obj = Instantiate(enemyActionCardPrefab);
                
                EnemyActionCard card = obj.GetComponent<EnemyActionCard>();
                card.SetActionCardData(placement.CardData);
                card.AddAttackValue(placement.AttackValue);
                card.AddDefenseValue(placement.DefenseValue);
                
                enemySlots[placement.SlotIndex].SetCurrentCard(card);
                _spawnedCards.Add(obj);
            }
        }

        private void HandleStageClear(StageClearEvent evt) => ClearEnemyCards();

        private void ClearEnemyCards()
        {
            foreach (GameObject card in _spawnedCards)
                if (card != null) Destroy(card);
            _spawnedCards.Clear();
            _pendingRespawns.Clear();

            foreach (EnemySlot slot in enemySlots)
                slot.RemoveCurrentCard();

            ClearSeals();
        }

        private void ClearSeals()
        {
            foreach (int slotIndex in _sealedSlots.Keys)
                if (slotIndex >= 0 && slotIndex < playerSlots.Count)
                    playerSlots[slotIndex].Unseal();
            _sealedSlots.Clear();
        }

        private void HandleEnemyCardDestroyed(EnemyCardDestroyedEvent evt)
        {
            if (_remainingRespawns <= 0) return;
            if (_currentData == null || _currentData.RespawnPool.Count == 0) return;

            _remainingRespawns--;
            _pendingRespawns.Add(_currentData.RespawnTurnDelay);
            enemyChannel.RaiseEvent(new RespawnCountChangedEvent().Init(_remainingRespawns, _currentData.MaxRespawnCount));
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            ResetSwaps();
            TickSealedSlots();

            for (int i = _pendingRespawns.Count - 1; i >= 0; i--)
            {
                if (_pendingRespawns[i] > 0)
                    _pendingRespawns[i]--;

                if (_pendingRespawns[i] > 0) continue;

                int slotIndex = GetRandomEmptySlotIndex();
                if (slotIndex < 0) continue; 

                EnemyRespawnEntry entry = _currentData.RespawnPool[Random.Range(0, _currentData.RespawnPool.Count)];
                SpawnRespawnEntry(entry, slotIndex);
                _pendingRespawns.RemoveAt(i);
            }
        }

        private bool TryConsumeSwap()
        {
            if (_remainingSwaps <= 0) return false;

            _remainingSwaps--;
            RaiseSwapCount();
            return true;
        }

        private void ResetSwaps()
        {
            _remainingSwaps = maxSwapsPerTurn;
            RaiseSwapCount();
        }

        private void RaiseSwapCount()
            => gameEventChannel.RaiseEvent(new SwapCountChangedEvent().Init(_remainingSwaps, maxSwapsPerTurn));

        private void SpawnRespawnEntry(EnemyRespawnEntry entry, int slotIndex)
        {
            if (entry == null || entry.CardData == null) return;
            if (slotIndex < 0 || slotIndex >= enemySlots.Count) return;

            GameObject obj = Instantiate(enemyActionCardPrefab);
            EnemyActionCard card = obj.GetComponent<EnemyActionCard>();
            card.SetActionCardData(entry.CardData);
            card.AddAttackValue(entry.AttackValue);
            card.AddDefenseValue(entry.DefenseValue);

            enemySlots[slotIndex].SetCurrentCard(card);
            _spawnedCards.Add(obj);
        }

        private int GetRandomEmptySlotIndex()
        {
            List<int> empty = new List<int>();
            for (int i = 0; i < enemySlots.Count; i++)
                if (enemySlots[i].CurrentCard == null)
                    empty.Add(i);
            return empty.Count == 0 ? -1 : empty[Random.Range(0, empty.Count)];
        }

        public bool SealRandomCardSlot(int turns)
        {
            List<int> candidates = new List<int>();
            for (int i = 0; i < playerSlots.Count; i++)
            {
                if (playerSlots[i].CurrentCard == null) continue;
                if (_sealedSlots.ContainsKey(i)) continue;
                candidates.Add(i);
            }

            if (candidates.Count == 0) return false;

            int slotIndex = candidates[Random.Range(0, candidates.Count)];
            playerSlots[slotIndex].Seal();
            _sealedSlots[slotIndex] = turns;
            return true;
        }

        private void TickSealedSlots()
        {
            if (_sealedSlots.Count == 0) return;

            List<int> keys = new List<int>(_sealedSlots.Keys);
            foreach (int slotIndex in keys)
            {
                _sealedSlots[slotIndex]--;
                if (_sealedSlots[slotIndex] > 0) continue;

                _sealedSlots.Remove(slotIndex);
                if (slotIndex >= 0 && slotIndex < playerSlots.Count)
                    playerSlots[slotIndex].Unseal();
            }
        }

        private void UpdateSlot(AbstractSlot abstractSlot, int slotNumber)
        {
            switch (abstractSlot.SlotType)
            {
                case SlotType.Player:
                    playerSlots[slotNumber] = (PlayerSlot)abstractSlot;
                    break;
                case SlotType.Enemy:
                    enemySlots[slotNumber] = (EnemySlot)abstractSlot;
                    break;
            }
        }
        
        public EnemyActionCard SpawnEnemyCardInFirstEmptySlot(ActionCardDataSO cardData, int attackValue, int defenseValue)
        {
            for (int i = 0; i < enemySlots.Count; i++)
            {
                if (enemySlots[i].CurrentCard != null) continue;
                return SpawnEnemyCard(cardData, attackValue, defenseValue, i);
            }
            return null;
        }
        
        public EnemyActionCard SpawnEnemyCard(ActionCardDataSO cardData, int attackValue, int defenseValue, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= enemySlots.Count) return null;
            if (enemySlots[slotIndex].CurrentCard != null) return null;

            GameObject obj = Instantiate(enemyActionCardPrefab);
            EnemyActionCard card = obj.GetComponent<EnemyActionCard>();
            card.SetActionCardData(cardData);
            card.AddAttackValue(attackValue);
            card.AddDefenseValue(defenseValue);

            enemySlots[slotIndex].SetCurrentCard(card);
            _spawnedCards.Add(obj);
            return card;
        }

        
        
        public List<AbstractSlot> GetTargetSlots(AbstractSlot curAbstractSlot, SlotTargetRangeType targetRangeType)
        {
            bool isPlayer = curAbstractSlot.SlotType == SlotType.Player;
            List<AbstractSlot> pSlots = new List<AbstractSlot>(playerSlots);
            List<AbstractSlot> eSlots = new List<AbstractSlot>(enemySlots);

            List<AbstractSlot> originSlots = isPlayer ? pSlots : eSlots;
            List<AbstractSlot> targetSlots = isPlayer ? eSlots : pSlots;

            int index = curAbstractSlot.SlotNumber;
            switch (targetRangeType)
            {
                case SlotTargetRangeType.FRONT_F:
                    return GetSlot(targetSlots, index);
                case SlotTargetRangeType.FRONT_LR:
                    return GetSlot(targetSlots, index - 1, index + 1);
                case SlotTargetRangeType.FRONT_ONE:
                    return GetSlot(targetSlots, 0);
                case SlotTargetRangeType.FRONT_TWO:
                    return GetSlot(targetSlots, 1);
                case SlotTargetRangeType.FRONT_THREE:
                    return GetSlot(targetSlots, 2);
                case SlotTargetRangeType.FRONT_FOUR:
                    return GetSlot(targetSlots, 3);
                case SlotTargetRangeType.SELF:
                    return GetSlot(originSlots, index);
                case SlotTargetRangeType.LEFT:
                    return GetSlot(originSlots, index - 1);
                case SlotTargetRangeType.RIGHT:
                    return GetSlot(originSlots, index + 1);
                case SlotTargetRangeType.RENDOM:
                    return GetSlot(targetSlots, Random.Range(0,4));
                case SlotTargetRangeType.ONE:
                    return GetSlot(targetSlots, 1);
                case SlotTargetRangeType.TWO:
                    return GetSlot(targetSlots, 2);
                case SlotTargetRangeType.THREE:
                    return GetSlot(targetSlots, 3);
                case SlotTargetRangeType.FOUR:
                    return GetSlot(targetSlots, 4);
                case SlotTargetRangeType.ALL:
                    return GetSlot(targetSlots,0,1,2,3);
            }
            return null;
        }

        private List<AbstractSlot> GetSlot(List<AbstractSlot> slots, params int[] targets)
        {
            var result = new List<AbstractSlot>();
            foreach (int i in targets)
                if (i >= 0 && i < slots.Count)
                    result.Add(slots[i]);
            return result;
        }
    }
}