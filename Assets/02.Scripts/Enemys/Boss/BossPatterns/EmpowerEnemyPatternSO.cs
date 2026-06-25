using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.ServiceLocatorSystem;
using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using _02.Scripts.SlotSystem.Slots;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "EmpowerEnemyPattern", menuName = "Enemy/Pattern/EmpowerEnemy")]
    public class EmpowerEnemyPatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private AbstractActionEffectSO effect;
        [SerializeField] private string particleName;
        [SerializeField] private float yOffset;

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            if (effect == null) return UniTask.CompletedTask;

            List<EnemySlot> withCards = new List<EnemySlot>();
            foreach (EnemySlot slot in context.SlotLogic.EnemySlots)
                if (slot.CurrentCard != null)
                    withCards.Add(slot);

            if (withCards.Count == 0) return UniTask.CompletedTask;

            EnemySlot target = withCards[Random.Range(0, withCards.Count)];
            if (target.CurrentCard is EnemyActionCard enemyCard)
                enemyCard.GrantEffect(effect);

            ServiceLocator.Get<IParticleService>().PlayParticle(particleName, target.transform.position + Vector3.up * yOffset);

            return UniTask.CompletedTask;
        }

        public override string GetDescription() => Description;
    }
}
