using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    // 좌우 인접 아군 카드를 강화한다. 각 카드의 타입에 맞춰(공/방) buffValue를 더한다.
    [CreateAssetMenu(fileName = "ChainBuffEffect", menuName = "Card/Action/Effect/ChainBuff")]
    public class ChainBuffActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private GradeValue buffValue;

        public override int GetDisplayValue(CardGrade grade) => buffValue.Get(grade);

        public override bool IsActivate(EffectExecuteContext context) => context.SourceSlot != null;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            SlotLogic slotLogic = FindFirstObjectByType<SlotLogic>();
            if (slotLogic == null) return UniTask.CompletedTask;

            int value = buffValue.Get(context.Grade);
            List<AbstractSlot> allies = GetAllySlots(slotLogic, context.SourceSlot);
            int index = context.SourceSlot.SlotNumber;

            Buff(GetCardAt(allies, index - 1), value);
            Buff(GetCardAt(allies, index + 1), value);
            return UniTask.CompletedTask;
        }

        private static void Buff(ActionCard card, int value)
        {
            if (card != null) card.ChangeValue(value);
        }

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
