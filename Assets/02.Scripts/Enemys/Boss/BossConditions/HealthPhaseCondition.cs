using System.Collections.Generic;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossConditions
{
    [CreateAssetMenu(fileName = "HealthPhaseCondition", menuName = "Enemy/Boss/Conditions/HealthPhase")]
    public class HealthPhaseCondition : AbstractEnemyConditionSO
    {
        [field: SerializeField] public List<float> HealthRatios { get; private set; } = new() { 0.75f, 0.5f, 0.25f };

        private readonly HashSet<float> _triggered = new();

        public override bool IsActivate(EnemyPatternContext context)
        {
            if (context.MaxHealth <= 0) return false;

            float ratio = (float)context.CurrentHealth / context.MaxHealth;
            foreach (float threshold in HealthRatios)
                if (ratio <= threshold && !_triggered.Contains(threshold))
                    return true;

            return false;
        }

        public override void NotifyTriggered(EnemyPatternContext context)
        {
            if (context.MaxHealth <= 0) return;

            float ratio = (float)context.CurrentHealth / context.MaxHealth;
            foreach (float threshold in HealthRatios)
                if (ratio <= threshold)
                    _triggered.Add(threshold);
        }

        public override void ResetRuntimeState() => _triggered.Clear();

        public override string GetDescription() => string.Format(Description, HealthRatios[0]*100f, HealthRatios[1]*100f, HealthRatios[2]*100f);
    }
}
