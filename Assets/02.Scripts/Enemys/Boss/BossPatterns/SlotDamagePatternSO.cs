using System;
using System.Collections.Generic;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SlotDamagePattern", menuName = "Enemy/Pattern/SlotDamage")]
    public class SlotDamagePatternSO : AbstractEnemyPatternSO
    {
        [SerializeField] private GameObject attackParticle;
        [SerializeField] private float yOffset;
        [SerializeField] private int damageValue;

        private PlayerSlot _currentTargetSlot;

        public override void Execute(EnemyPatternContext context, Action onComplete = null)
        {
            var targets = new List<PlayerSlot>();
            foreach (var slot in context.SlotLogic.PlayerSlots)
                if (slot.CurrentCard != null) targets.Add(slot);

            ExecuteChain(targets, 0, context, onComplete);
        }

        private void ExecuteChain(List<PlayerSlot> targets, int index, EnemyPatternContext context, Action onComplete)
        {
            Debug.Log($"index: {index}, count: {targets.Count}  ");
            if (index >= targets.Count)
            {
                onComplete?.Invoke();
                return;
            }

            _currentTargetSlot = targets[index];
            base.Execute(context, () => ExecuteChain(targets, index + 1, context, onComplete));
        }

        protected override void ExecutePattern(EnemyPatternContext context, Action onComplete)
        {
            if (_currentTargetSlot?.CurrentCard == null) { onComplete?.Invoke(); return; }

            Vector3 spawnPos = _currentTargetSlot.CurrentCard.transform.position + Vector3.up * yOffset;
            Instantiate(attackParticle, spawnPos, Quaternion.identity);
            _currentTargetSlot.CurrentCard.TakeDamage(damageValue);
            onComplete?.Invoke();
        }
        
        public override string GetDescription() => string.Format(Description, damageValue);
    }
}
