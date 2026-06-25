using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "TypeSynergyEffect", menuName = "Card/Action/Effect/TypeSynergy")]
    public class TypeSynergyActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private GradeValue bonusValue;

        public override int GetDisplayValue(CardGrade grade) => bonusValue.Get(grade);

        public override bool IsActivate(EffectExecuteContext context)
        {
            if (context.SourceSlot == null) return false;

            SlotLogic slotLogic = FindFirstObjectByType<SlotLogic>();
            if (slotLogic == null) return false;

            List<AbstractSlot> allies = GetAllySlots(slotLogic, context.SourceSlot);
            int index = context.SourceSlot.SlotNumber;

            return (IsAttackCard(GetCardAt(allies, index - 1))
                   && IsAttackCard(GetCardAt(allies, index + 1))) ||
                   (IsDefenseCard(GetCardAt(allies, index - 1)) &&
                    IsDefenseCard(GetCardAt(allies, index + 1)));
        }

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.ChangeValue(bonusValue.Get(context.Grade));
            return UniTask.CompletedTask;
        }

        private static bool IsAttackCard(ActionCard card)
            => card != null && card.ActionCardData.ActionCardType == ActionCardType.Attack;
        private static bool IsDefenseCard(ActionCard card)
            => card != null && card.ActionCardData.ActionCardType == ActionCardType.Defense;

        private static List<AbstractSlot> GetAllySlots(SlotLogic slotLogic, AbstractSlot source)
        {
            List<AbstractSlot> result = new List<AbstractSlot>();
            if (source.SlotType == SlotType.Player) result.AddRange(slotLogic.PlayerSlots);
            else result.AddRange(slotLogic.EnemySlots);
            return result;
        }

        private static ActionCard GetCardAt(List<AbstractSlot> slots, int slotNumber)
        {
            foreach (AbstractSlot slot in slots)
                if (slot.SlotNumber == slotNumber) return slot.CurrentCard;
            return null;
        }
    }
}
