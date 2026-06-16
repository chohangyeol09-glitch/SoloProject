using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "PlayUnderHealth", menuName = "Enemy/Boss/Conditions/PlayUnderHealth")]
    public class PlayUnderHealth : AbstractEnemyConditionSO
    {
        [field: SerializeField] public float HealthRatio { get; private set; } = 0.5f;

        public override bool IsActivate(EnemyPatternContext context)
            => (float)context.CurrentHealth / context.MaxHealth <= HealthRatio;

        public override string GetDescription()
        {
            return string.Format(Description, HealthRatio * 100);
        }
    }
}