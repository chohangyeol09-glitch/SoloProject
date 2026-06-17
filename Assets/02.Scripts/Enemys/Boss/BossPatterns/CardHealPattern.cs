using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "CardHealPattern", menuName = "Enemy/Pattern/CardHeal", order = 0)]
    public class CardHealPattern : AbstractEnemyPatternSO
    {
        [SerializeField] private int amount;
        [SerializeField] private GameObject particle;
        [SerializeField] private float yOffset;
        
        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            int r;

            do
            {
                r = Random.Range(0, context.SlotLogic.EnemySlots.Count);
            } while (context.SlotLogic.EnemySlots[r] == null);

            context.SlotLogic.EnemySlots[r].CurrentCard.AddDefenseValue(amount);
            Instantiate(particle, context.SlotLogic.EnemySlots[r].transform.position + Vector3.up * yOffset, Quaternion.identity);

            return UniTask.CompletedTask;
        }
        
        public override string GetDescription() => string.Format(Description, amount);
    }
}