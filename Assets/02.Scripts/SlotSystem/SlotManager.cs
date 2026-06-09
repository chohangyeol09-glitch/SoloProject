using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvent;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.Enemy;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class SlotManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerSlot> playerSlots = new();
        [SerializeField] private List<EnemySlot> enemySlots = new();
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private GameObject enemyActionCardPrefab;

        private List<GameObject> _spawnedCards = new();

        private void Awake()
        {
            foreach (PlayerSlot slot in playerSlots)
                slot.OnDropCard += UpdateSlot;

            turnEventChannel.AddListener<ExecuteCardsEvent>(HandleExecuteCards);
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
        }

        private void OnDestroy()
        {
            turnEventChannel.RemoveListener<ExecuteCardsEvent>(HandleExecuteCards);
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            ClearEnemyCards();
            Debug.Log(evt.EnemyData.EnemyName);
            foreach (EnemyCardPlacement placement in evt.EnemyData.CardPlacements)
            {
                if (placement.SlotIndex < 0 || placement.SlotIndex >= enemySlots.Count) continue;
                if (placement.CardData == null) continue;
                Debug.Log("ccc");

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

        private void HandleExecuteCards(ExecuteCardsEvent evt)
            => StartCoroutine(ExecuteAndNotify(evt.OnComplete));

        private IEnumerator ExecuteAndNotify(Action onComplete)
        {
            yield return StartCoroutine(ExecuteCardsInOrder());
            onComplete?.Invoke();
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

        private IEnumerator ExecuteCardsInOrder()
        {
            foreach (PlayerSlot slot in playerSlots)
            {
                if (slot.CurrentCard == null) continue;
                bool done = false;
                ExecuteCard(slot, () => done = true);
                yield return new WaitUntil(() => done);
            }

            foreach (EnemySlot slot in enemySlots)
            {
                if (slot.CurrentCard == null) continue;
                bool done = false;
                ExecuteCard(slot, () => done = true);
                yield return new WaitUntil(() => done);
            }
        }

        private void ExecuteCard(AbstractSlot slot, Action onComplete)
        {
            ActionCard card = slot.CurrentCard;
            Debug.Log(card == null);
            
            List<AbstractSlot> targets = GetTargetSlots(slot, card.ActionCardData.TargetRangeType);

            EffectExecuteContext context = new EffectExecuteContext(card, targets);

            foreach (AbstractActionEffectSO effect in card.ActionCardData.BeforeEffects)
                if (effect.IsActivate(context))
                    effect.Apply(context, targets);

            card.ActionCardData.Action.Execute(card, targets, onComplete);

            foreach (AbstractActionEffectSO effect in card.ActionCardData.AfterEffects)
                if (effect.IsActivate(context))
                    effect.Apply(context, targets);
        }

        private List<AbstractSlot> GetTargetSlots(AbstractSlot curAbstractSlot, SlotTargetRangeType targetRangeType)
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

    #if UNITY_EDITOR
        [ContextMenu("Test Attack")]
        private void TestAttack() => StartCoroutine(ExecuteCardsInOrder());
    #endif
    }
}