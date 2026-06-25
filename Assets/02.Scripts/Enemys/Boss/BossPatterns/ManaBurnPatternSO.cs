using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "ManaBurnPattern", menuName = "Enemy/Pattern/ManaBurn")]
    public class ManaBurnPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private int burnAmount = 1;
        [SerializeField] private EventChannelSO playerChannel;

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            playerChannel.RaiseEvent(new BurnCostEvent().Init(burnAmount));
            return UniTask.CompletedTask;
        }

        public override string GetDescription() => string.Format(Description, burnAmount);
    }
}
