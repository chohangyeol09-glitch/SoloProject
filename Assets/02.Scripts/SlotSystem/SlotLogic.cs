using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.Enemys;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class SlotLogic : MonoBehaviour, IModule
    {
        [SerializeField] private List<PlayerSlot> playerSlots = new();
        [SerializeField] private List<EnemySlot> enemySlots = new();
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private GameObject enemyActionCardPrefab;

        public IReadOnlyList<PlayerSlot> PlayerSlots => playerSlots;
        public IReadOnlyList<EnemySlot> EnemySlots => enemySlots;
        
        private List<GameObject> _spawnedCards = new();
        private ModuleOwner _owner;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            
            foreach (PlayerSlot slot in playerSlots)
                slot.OnDropCard += UpdateSlot;

            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            ClearEnemyCards();
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

            foreach (EnemySlot slot in enemySlots)
                slot.RemoveCurrentCard();
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
                case SlotTargetRangeType.SELF:
                    return GetSlot(originSlots, index);
                case SlotTargetRangeType.LEFT:
                    return GetSlot(originSlots, index - 1);
                case SlotTargetRangeType.RIGHT:
                    return GetSlot(originSlots, index + 1);
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