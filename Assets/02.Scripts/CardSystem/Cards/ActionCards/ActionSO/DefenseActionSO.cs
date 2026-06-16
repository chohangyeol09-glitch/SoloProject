using System;
using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "DefenseAction", menuName = "Card/Action/DefenseAction")]
    public class DefenseActionSO : AbstractActionSO
    {
        [SerializeField] private float riseHeight = 1f;
        [SerializeField] private float riseDuration = 0.2f;
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private float returnDuration = 0.2f;
        [SerializeField] private GameObject shieldParticlePrefab;

        // During the card-action(turn) phase the defense card does not act.
        // The defense is already granted when the card receives its value
        // (player: when a stat card is placed via PlayGrant; enemy: at spawn).
        public override UniTask Execute(ActionCard card, List<AbstractSlot> targets) => UniTask.CompletedTask;

        // Called when a stat card is placed on this defense card.
        // Plays the same kind of motion as the attack action and grants defense to each target.
        public void PlayGrant(ActionCard card, List<AbstractSlot> targets, int defenseValue, Action onComplete = null)
        {
            if (targets == null || targets.Count == 0 || defenseValue <= 0) { onComplete?.Invoke(); return; }

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
                    SpawnShieldParticle(capturedTarget.transform.position);
                    capturedTarget.CurrentCard?.AddDefenseValue(defenseValue);
                });

                seq.Append(card.transform.DOMove(risePos, returnDuration));
                seq.AppendCallback(() =>
                    card.transform.DORotateQuaternion(slotRot, returnDuration));
                seq.AppendInterval(returnDuration);
            }

            seq.Append(card.transform.DOMove(slotPos, returnDuration));
            seq.Join(card.transform.DORotateQuaternion(slotRot, returnDuration));
            seq.OnComplete(() => onComplete?.Invoke());
        }

        private void SpawnShieldParticle(Vector3 position)
        {
            if (shieldParticlePrefab == null) return;
            GameObject ps = Instantiate(shieldParticlePrefab, position, Quaternion.identity);
            ParticleSystem particle = ps.GetComponent<ParticleSystem>();
            float lifetime = particle.main.duration + particle.main.startLifetime.constantMax;
            Destroy(ps, lifetime);
        }
    }
}
