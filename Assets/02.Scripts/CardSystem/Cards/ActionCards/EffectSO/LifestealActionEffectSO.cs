using System.Collections.Generic;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "LifestealEffect", menuName = "Card/Action/Effect/Lifesteal")]
    public class LifestealActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private GradeFloat ratio;

        // ratio(0~1)를 % 정수로 표시
        public override int GetDisplayValue(CardGrade grade) => Mathf.RoundToInt(ratio.Get(grade) * 100f);

        // 플레이어 카드만, 공격값이 있을 때 흡혈
        public override bool IsActivate(EffectExecuteContext context)
            => context.ActionCard is PlayerActionCard && context.ActionCard.AttackValue > 0;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            int heal = Mathf.CeilToInt(context.ActionCard.AttackValue * ratio.Get(context.Grade));
            if (heal > 0)
                playerChannel.RaiseEvent(new HealEvent().Init(heal));
            return UniTask.CompletedTask;
        }
    }
}
