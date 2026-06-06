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
    [SerializeField] private float riseHeight = 1f;      // 공중에 뜨는 높이
    [SerializeField] private float riseDuration = 0.2f;  // 뜨는 시간
    [SerializeField] private float moveDuration = 0.3f;  // 이동 시간
    [SerializeField] private float returnDuration = 0.2f;// 복귀 시간

    public override void Execute(ActionCard card, List<AbstractSlot> targets, Action onComplete = null)
    {
        if (targets == null || targets.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }
        
        if (card.AttackValue <= 0)
        {
            onComplete?.Invoke(); // ← 추가
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
                    // 카드 기본 rotation 고려
                    Quaternion baseRot = Quaternion.Euler(-90, 0, 0); // 카드 기본 rotation
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
    
                if (capturedTarget.CurrentCard != null)
                {
                    // 카드 있으면 카드에 데미지
                    capturedTarget.CurrentCard.TakeDamage(card.AttackValue, false);
                    capturedTarget.CurrentCard.PlayHitShake(card.AttackValue);
                }
                else
                {
                    // 카드 없으면 슬롯 타입에 따라 플레이어/적에게 직접 데미지
                    if (capturedTarget.SlotType == SlotType.Player)
                        card.RaisePlayerDamage(card.AttackValue);
                    else if (capturedTarget.SlotType == SlotType.Enemy)
                        card.RaiseEnemyDamage(card.AttackValue);
                }
            });
            
            seq.Append(card.transform.DOMove(risePos, returnDuration));

            seq.AppendCallback(() =>
            {
                card.transform.DORotateQuaternion(slotRot, returnDuration);
            });
            seq.AppendInterval(returnDuration);
        }

        seq.Append(card.transform.DOMove(slotPos, returnDuration));
        seq.Join(card.transform.DORotateQuaternion(slotRot, returnDuration));
        seq.OnComplete(() => onComplete?.Invoke());
    }
}
}