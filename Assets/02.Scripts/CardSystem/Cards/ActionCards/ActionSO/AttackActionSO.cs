using System;
using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Card/Action/AttackAction")]
    public class AttackActionSO : AbstractActionSO
    {
        [Header("Move")] [SerializeField] private float riseHeight = 1f;
        [SerializeField] private float riseDuration = 0.2f;
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private float returnDuration = 0.2f;

        [Header("Effect")] [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject slashEffectPrefab;

        public override void Execute(ActionCard card, List<AbstractSlot> targets, Action onComplete = null)
        {
            if (targets == null || targets.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            if (card.AttackValue <= 0)
            {
                onComplete?.Invoke();
                return;
            }

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

                seq.AppendCallback(() =>
                {
                    if (slashEffectPrefab != null)
                    {
                        GameObject fx = Instantiate(
                            slashEffectPrefab,
                            card.transform.position,
                            card.transform.rotation
                        );
                        Destroy(fx, 2f);
                    }
                });

                seq.Append(card.transform.DOMove(
                    capturedTarget.transform.position + Vector3.up * riseHeight,
                    moveDuration));

                seq.AppendCallback(() =>
                {
                    if (capturedTarget == null) return;

                    if (hitEffectPrefab != null)
                    {
                        GameObject fx = Instantiate(
                            hitEffectPrefab,
                            capturedTarget.transform.position,
                            Quaternion.identity
                        );
                        Destroy(fx, 2f);
                    }

                    if (capturedTarget.CurrentCard != null)
                    {
                        capturedTarget.CurrentCard.TakeDamage(card.AttackValue, false);
                        capturedTarget.CurrentCard.PlayHitShake(card.AttackValue);
                    }
                    else
                    {
                        if (capturedTarget.SlotType == SlotType.Player)
                            card.RaisePlayerDamage(card.AttackValue);
                        else if (capturedTarget.SlotType == SlotType.Enemy)
                            card.RaiseEnemyDamage(card.AttackValue);
                    }
                });

                seq.Append(card.transform.DOMove(risePos, returnDuration));
                seq.AppendCallback(() => { card.transform.DORotateQuaternion(slotRot, returnDuration); });
                seq.AppendInterval(returnDuration);
            }

            seq.Append(card.transform.DOMove(slotPos, returnDuration));
            seq.Join(card.transform.DORotateQuaternion(slotRot, returnDuration));
            seq.OnComplete(() => onComplete?.Invoke());
        }
    }
}