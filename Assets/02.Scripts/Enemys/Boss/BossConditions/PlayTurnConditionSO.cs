using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "PlayTurnCondition", menuName = "Enemy/Boss/Conditions/PlayTurn")]
    public class PlayTurnConditionSO : AbstractEnemyConditionSO
    {
        [field: SerializeField] public int PlayTurn { get; private set; } = 3;

        public override bool IsActivate(EnemyPatternContext context)
        {
            return context.CurrentTurn == PlayTurn;
        }

        public override string GetDescription()
        {
            return string.Format(Description, PlayTurn);
        }
    }
}