using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionWeapon;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.SlotSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Card/Action/AttackAction")]
    public class AttackActionSO : AbstractActionSO
    {
        [SerializeField] private float riseHeight = 1f;
        [SerializeField] private float riseDuration = 0.2f;
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private float returnDuration = 0.2f;
        [SerializeField] private GameObject hitParticlePrefab;

        public override async UniTask Execute(ActionCard card, List<AbstractSlot> targets)
        {
            if (targets == null || targets.Count == 0) return;
            if (card.AttackValue <= 0) return;

            Vector3 slotPos = card.transform.position;
            Quaternion slotRot = card.transform.rotation;
            Vector3 risePos = slotPos + Vector3.up * riseHeight;

            Sequence seq = DOTween.Sequence();
            seq.Append(card.transform.DOMove(risePos, riseDuration));

            foreach (AbstractSlot target in targets)
            {
                AbstractSlot capturedTarget = target;

                seq.AppendCallback(() =>
                {
                    if (capturedTarget == null) return;
                    Vector3 dir = capturedTarget.transform.position - card.transform.position;
                    dir.y = 0;
                    if (dir != Vector3.zero)
                    {
                        Quaternion baseRot = Quaternion.Euler(-90, 0, 0);
                        Quaternion lookRot = Quaternion.LookRotation(dir) * baseRot;
                        card.transform.DORotateQuaternion(lookRot, riseDuration);
                    }
                });
                seq.AppendInterval(riseDuration);

                seq.Append(card.transform.DOMove(
                    capturedTarget.transform.position + Vector3.up * riseHeight,
                    moveDuration));

                seq.AppendCallback(() =>
                {
                    if (capturedTarget == null) return;

                    SpawnHitParticle(capturedTarget.transform.position);

                    if (capturedTarget.CurrentCard != null)
                    {
                        if (card is EnemyActionCard)
                        {
                            int overflow = capturedTarget.CurrentCard.GetOverflowDamage(card.AttackValue);
                            if (overflow > 0) card.RaisePlayerDamage(overflow);
                        }
                        else
                        {
                            capturedTarget.CurrentCard.TakeDamage(card.AttackValue, false);
                        }
                        capturedTarget.CurrentCard.PlayHitShake(card.AttackValue);
                    }
                    else
                    {
                        if (capturedTarget.SlotType == SlotType.Player && card is EnemyActionCard)
                            card.RaisePlayerDamage(card.AttackValue);
                        else if (capturedTarget.SlotType == SlotType.Enemy)
                            card.RaiseEnemyDamage(card.AttackValue);
                    }
                });

                seq.Append(card.transform.DOMove(risePos, returnDuration));
                seq.AppendCallback(() =>
                    card.transform.DORotateQuaternion(slotRot, returnDuration));
                seq.AppendInterval(returnDuration);
            }

            seq.Append(card.transform.DOMove(slotPos, returnDuration));
            seq.Join(card.transform.DORotateQuaternion(slotRot, returnDuration));
            await seq.ToUniTask();
        }
        private void SpawnHitParticle(Vector3 position)
        {
            if (hitParticlePrefab == null) return;
            GameObject ps = Instantiate(hitParticlePrefab, position, Quaternion.identity);
            ParticleSystem particle = ps.GetComponent<ParticleSystem>();
            float lifetime = particle.main.duration + particle.main.startLifetime.constantMax; 
            Destroy(ps, lifetime);
        }
    }
}