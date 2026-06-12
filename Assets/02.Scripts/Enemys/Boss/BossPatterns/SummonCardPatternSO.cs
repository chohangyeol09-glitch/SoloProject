using System;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SummonCardPattern", menuName = "Enemy/Boss/Pattern/SummonCard")]
    public class SummonCardPatternSO : AbstractBossPatternSO
    {
        [field: SerializeField] public ActionCardDataSO CardData { get; private set; }
        [field: SerializeField] public int AttackValue { get; private set; }
        [field: SerializeField] public int DefenseValue { get; private set; }

        protected override void ExecutePattern(BossGimmickContext context, Action onComplete)
        {
            Debug.LogWarning("context is null? : " + context == null);
            Debug.Log($"context: {context != null}, SlotLogic: {context?.SlotLogic != null}");
            context.SlotLogic.SpawnEnemyCardInFirstEmptySlot(CardData, AttackValue, DefenseValue);
            onComplete?.Invoke();
        }
    }
}