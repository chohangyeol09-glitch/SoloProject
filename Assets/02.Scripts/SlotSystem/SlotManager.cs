using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class SlotManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerSlot> playerSlots = new();
        [SerializeField] private List<EnemySlot> enemySlots  = new();

        private void Awake()
        {
            foreach (PlayerSlot slot in playerSlots)
                slot.OnDropCard += UpdateSlot;

            /*foreach (AbstractSlot slot in enemySlots)
                slot.OnDropCard += UpdateSlot;*/
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

        public void ExecuteAllCard()
        {
            foreach (PlayerSlot slot in playerSlots)
                ExecuteCard(slot);
            
            foreach (EnemySlot slot in enemySlots)
                ExecuteCard(slot);
        }

        private void ExecuteCard(AbstractSlot abstractSlot)
        {
            ActionCard card = abstractSlot.CurrentCard;
            List<AbstractSlot> targets = GetTargetSlots(abstractSlot, abstractSlot.CurrentCard.ActionCadeData.TargetRangeType);
            
            card.ActionCadeData.Action.Execute(card, targets);
            EffectExecuteContext context = new EffectExecuteContext(card, targets);
            foreach (AbstractActionEffectSO effect in card.ActionCadeData.Effects)
            {
                if (effect.IsActivate(context))
                    effect.Apply(context, targets);
            }
            
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
                //상대 타겟
                case SlotTargetRangeType.FRONT:
                    return GetSlot(targetSlots, index);
                
                case SlotTargetRangeType.FRONT_LR:
                    return GetSlot(targetSlots, index - 1, index + 1);
                
                //우리쪽 타겟
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
        private void TestAttack()
        {
            foreach (AbstractSlot slot in playerSlots)
            {
                
            }
        }
        
        #endif
        
    }
}
