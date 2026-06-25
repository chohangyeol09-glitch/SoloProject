using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "EnemyCardCountCondition", menuName = "Enemy/Boss/Conditions/EnemyCardCount")]
    public class EnemyCardCountCondition : AbstractEnemyConditionSO
    {
        [field: SerializeField] public int MaxCardCount { get; private set; } = 1;

        public override bool IsActivate(EnemyPatternContext context)
        {
            if (context.SlotLogic == null) return false;

            int alive = 0;
            foreach (var slot in context.SlotLogic.EnemySlots)
                if (slot.CurrentCard != null) alive++;

            return alive <= MaxCardCount;
        }

        public override string GetDescription() => string.Format(Description, MaxCardCount);
    }
}
