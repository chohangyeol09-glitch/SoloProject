using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "EveryNTurnsCondition", menuName = "Enemy/Boss/Conditions/EveryNTurns")]
    public class EveryNTurnsCondition : AbstractEnemyConditionSO
    {
        [field: SerializeField] public int Interval { get; private set; } = 3;

        public override bool IsActivate(EnemyPatternContext context)
        {
            if (Interval <= 0) return false;
            if (context.CurrentTurn <= 0) return false;
            return context.CurrentTurn % Interval == 0;
        }

        public override string GetDescription() => string.Format(Description, Interval);
    }
}
