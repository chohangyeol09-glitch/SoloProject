using System;
using System.Collections;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SlotDamagePattern", menuName = "Enemy/Boss/Pattern/SlotDamage")]
    public class SlotDamagePatternSO : AbstractBossPatternSO
    {
        [SerializeField] private GameObject attackParticle;
        [SerializeField] private float yOffset;
        [SerializeField] private int damageValue;
        [SerializeField] private float delay;

        protected override void ExecutePattern(BossGimmickContext context, Action onComplete)
        {
            context.SlotLogic.StartCoroutine(ExecuteDelay(context, onComplete));
        }

        private IEnumerator ExecuteDelay(BossGimmickContext context, Action onComplete)
        {
            foreach (PlayerSlot slot in context.SlotLogic.PlayerSlots)
            {
                if (slot.CurrentCard == null) continue;

                Vector3 spawnPos = slot.CurrentCard.transform.position + Vector3.up * yOffset;
                Instantiate(attackParticle, spawnPos, Quaternion.identity);
                slot.CurrentCard.TakeDamage(damageValue);
                yield return new WaitForSeconds(delay);
            }

            onComplete?.Invoke();
        }
    }
}