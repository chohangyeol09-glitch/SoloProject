using Cysharp.Threading.Tasks;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.CoreSystem.ServiceLocatorSystem;
using _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SlotDamagePattern", menuName = "Enemy/Pattern/SlotDamage")]
    public class SlotDamagePatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private string attackParticleName;
        [SerializeField] private float yOffset;
        [SerializeField] private int damageValue;
        [SerializeField] private EventChannelSO playerChannel;

        private PlayerSlot _currentTargetSlot;

        public override async UniTask Execute(EnemyPatternContext context)
        {
            foreach (PlayerSlot target in context.SlotLogic.PlayerSlots)
            {
                _currentTargetSlot = target;
                await base.Execute(context);
            }
        }

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            if (_currentTargetSlot == null) return UniTask.CompletedTask;

            if (_currentTargetSlot.CurrentCard != null)
            {
                Vector3 spawnPos = _currentTargetSlot.CurrentCard.transform.position + Vector3.up * yOffset;
                ServiceLocator.Get<IParticleService>().PlayParticle(attackParticleName, spawnPos);
                _currentTargetSlot.CurrentCard.TakeDamage(damageValue);
            }
            else
            {
                Vector3 spawnPos = _currentTargetSlot.transform.position + Vector3.up * yOffset;
                ServiceLocator.Get<IParticleService>().PlayParticle(attackParticleName, spawnPos);
                playerChannel.RaiseEvent(new TakeDamageEvent().Init(damageValue));
            }

            return UniTask.CompletedTask;
        }
        
        public override string GetDescription() => string.Format(Description, damageValue);
    }
}
