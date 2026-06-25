using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SlotSealPattern", menuName = "Enemy/Pattern/SlotSeal")]
    public class SlotSealPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private int sealDuration = 2;

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            context.SlotLogic.SealRandomCardSlot(sealDuration);
            return UniTask.CompletedTask;
        }

        public override string GetDescription() => string.Format(Description, sealDuration);
    }
}
