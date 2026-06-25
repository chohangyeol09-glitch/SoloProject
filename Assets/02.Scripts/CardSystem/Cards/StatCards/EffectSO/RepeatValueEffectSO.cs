using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "RepeatValue", menuName = "Card/Stat/Effect/RepeatValue", order = 0)]
    public class RepeatValueEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private GradeValue repeatCount;
        [SerializeField] private GradeValue value;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { repeatCount.Get(grade), value.Get(grade) };

        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            CardGrade grade = GetCardGrade(context);
            int count = repeatCount.Get(grade);
            int val = value.Get(grade);
            for (int i = 0; i < count; i++)
                context.TargetActionCard.ChangeValue(val);
        }
    }
}
