using System;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "Heal", menuName = "Enemy/Pattern/Heal", order = 0)]
    public class HealPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private int amount;

        protected override void ExecutePattern(EnemyPatternContext context, Action onComplete)
        {
            Enemy.Instance.Heal(amount);
        }
        public override string GetDescription() => string.Format(Description, amount);
        
    }
}