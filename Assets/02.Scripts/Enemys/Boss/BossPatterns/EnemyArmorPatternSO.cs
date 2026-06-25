using _02.Scripts.CoreSystem.ServiceLocatorSystem;
using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using _02.Scripts.SlotSystem.Slots;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "EnemyArmorPattern", menuName = "Enemy/Pattern/EnemyArmor")]
    public class EnemyArmorPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private int amount = 1;
        [SerializeField] private string particleName;
        [SerializeField] private float yOffset;

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            foreach (EnemySlot slot in context.SlotLogic.EnemySlots)
            {
                if (slot.CurrentCard == null) continue;
                slot.CurrentCard.AddDefenseValue(amount);
                ServiceLocator.Get<IParticleService>().PlayParticle(particleName, slot.transform.position + Vector3.up * yOffset);
            }

            return UniTask.CompletedTask;
        }

        public override string GetDescription() => string.Format(Description, amount);
    }
}
