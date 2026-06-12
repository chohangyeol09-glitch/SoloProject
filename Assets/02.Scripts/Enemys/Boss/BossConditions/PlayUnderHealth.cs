using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "PlayUnderHelath", menuName = "Enemy/Boss/Conditions/PlayUnderHealth")]
    public class PlayUnderHealth : AbstractBossConditionSO
    {
        [field: SerializeField] public float HealthRatio { get; private set; } = 0.5f;

        public override bool IsActivate(BossGimmickContext context)
            => (float)context.CurrentHealth / context.MaxHealth <= HealthRatio;
    }
}