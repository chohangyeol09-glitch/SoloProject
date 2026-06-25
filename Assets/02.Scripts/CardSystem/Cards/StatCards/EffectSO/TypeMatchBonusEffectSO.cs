using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "TypeMatchBonus", menuName = "Card/Stat/Effect/TypeMatchBonus", order = 0)]
    public class TypeMatchBonusEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private ActionCardType matchType;
        [SerializeField] private GradeValue bonusValue;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { bonusValue.Get(grade) };

        // 대상 카드의 타입이 matchType과 같을 때만 발동
        public override bool IsActivate(StatExecuteContext context)
            => context.TargetActionCard.ActionCardData.ActionCardType == matchType;

        public override void Apply(StatExecuteContext context)
        {
            // 발동 조건상 카드 타입 == matchType 이므로 ChangeValue가 해당 타입에 더한다.
            context.TargetActionCard.ChangeValue(bonusValue.Get(GetCardGrade(context)));
        }
    }
}
