using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    public abstract class AbstractStatEffectSO : ScriptableObject
    {
        public abstract bool IsActivate(StatExecuteContext context);
        public abstract void Apply(StatExecuteContext context);

        // 스탯카드 자신의 등급 (등급별 값 선택에 사용)
        protected CardGrade GetCardGrade(StatExecuteContext context)
            => context.StatCard.StatData.Grade;

        // 카드 설명({0},{1}...)에 넣을 값들. 등급별 값은 grade로 선택한다.
        public virtual object[] GetDescriptionArgs(CardGrade grade) => System.Array.Empty<object>();
    }
}