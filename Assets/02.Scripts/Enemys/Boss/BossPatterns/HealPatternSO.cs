using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "Heal", menuName = "Enemy/Pattern/Heal", order = 0)]
    public class HealPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private int amount;

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            Enemy.Instance.Heal(amount);
            return UniTask.CompletedTask;
        }
        public override string GetDescription() => string.Format(Description, amount);
        
    }
}