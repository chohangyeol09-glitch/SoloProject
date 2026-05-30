using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class SlotManager : MonoBehaviour
    {
        [SerializeField] private List<Slot> playerSlots = new();
        [SerializeField] private List<Slot> enemySlots  = new();

        private void Awake()
        {
            foreach (Slot slot in playerSlots)
                slot.OnDropCard += UpdateSlot;

            foreach (Slot slot in enemySlots)
                slot.OnDropCard += UpdateSlot;
        }

        private void UpdateSlot(Slot slot, int slotNumber)
        {
            switch (slot.SlotType)
            {
                case SlotType.Player:
                    playerSlots[slotNumber] = slot;
                    break;
                case SlotType.Enemy:
                    enemySlots[slotNumber] = slot;
                    break;
            }
        }

        public void ExecuteAllCard()
        {
            foreach (Slot slot in playerSlots)
                ExecuteCard(slot);
        }

        private void ExecuteCard(Slot slot)
        {
            if (slot.CurrentCard is not ActionCard card) return;
            
            List<Slot> targets = GetTargetSlots(slot, slot.CurrentCard.ActionCadeData.TargetRangeType);
            
            card.ActionCadeData.Action.Execute(card, targets);
            EffectExecuteContext context = new EffectExecuteContext(card, targets);
            foreach (AbstractActionEffectSO effect in card.ActionCadeData.Effects)
            {
                if (effect.IsActivate(context))
                    effect.Apply(context, targets);
            }
            
        }

        private List<Slot> GetTargetSlots(Slot curSlot, SlotTargetRangeType targetRangeType)
        {
            bool isPlayer = curSlot.SlotType == SlotType.Player;
            List<Slot> originSlots = isPlayer ? playerSlots : enemySlots;
            List<Slot> targetSlots = isPlayer ? enemySlots : playerSlots;

            int index = curSlot.SlotNumber;
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

        private List<Slot> GetSlot(List<Slot> slots, params int[] targets)
        {
            var result = new List<Slot>();
            foreach (int i in targets)
                if (i >= 0 && i < slots.Count)
                    result.Add(slots[i]);
            return result;
        }

#if UNITY_EDITOR

        [ContextMenu("Test Attack")]
        private void TestAttack()
        {
            foreach (Slot slot in playerSlots)
            {
                
            }
        }
        
        #endif
        
    }
}
