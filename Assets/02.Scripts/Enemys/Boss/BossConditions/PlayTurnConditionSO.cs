using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "PlayTurnCondition", menuName = "Enemy/Boss/Conditions/PlayTurn")]
    public class PlayTurnConditionSO : AbstractBossConditionSO
    {
        [field: SerializeField] public int PlayTurn { get; private set; } = 3;

        public override bool IsActivate(BossGimmickContext context)
        {
            Debug.Log($"TurnCondition| CurrentTurn: {context.CurrentTurn}, PlayTurn: {PlayTurn}, Ture: {context.CurrentTurn == PlayTurn}");
            return context.CurrentTurn == PlayTurn;
        }
    }
}