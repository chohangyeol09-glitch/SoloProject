using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "SplashTargetEffect", menuName = "Card/Action/Effect/SplashTarget")]
    public class SplashTargetActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private GradeValue spreadRange;

        public override int GetDisplayValue(CardGrade grade) => spreadRange.Get(grade);

        public override bool IsActivate(EffectExecuteContext context)
            => context.TargetSlots != null && context.TargetSlots.Count > 0;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            int range = spreadRange.Get(context.Grade);
            if (range <= 0) return UniTask.CompletedTask;

            SlotLogic slotLogic = FindFirstObjectByType<SlotLogic>();
            if (slotLogic == null) return UniTask.CompletedTask;

            List<AbstractSlot> all = new List<AbstractSlot>();
            all.AddRange(slotLogic.PlayerSlots);
            all.AddRange(slotLogic.EnemySlots);

            List<AbstractSlot> origin = new List<AbstractSlot>(targets);
            foreach (AbstractSlot target in origin)
            {
                foreach (AbstractSlot slot in all)
                {
                    if (slot.SlotType != target.SlotType) continue;
                    int diff = Mathf.Abs(slot.SlotNumber - target.SlotNumber);
                    if (diff == 0 || diff > range) continue;
                    if (!targets.Contains(slot)) targets.Add(slot);
                }
            }
            return UniTask.CompletedTask;
        }
    }
}
