using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    // 0 ~ maxValue(등급별) 사이 랜덤값을 뽑아 대상 카드에 더한다.
    [CreateAssetMenu(fileName = "RandomValue", menuName = "Card/Stat/Effect/RandomValue", order = 0)]
    public class RandomValueEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private GradeValue maxValue;

        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            int max = maxValue.Get(GetCardGrade(context));
            int bonus = Random.Range(0, max + 1); // 0 ~ max 포함
            context.TargetActionCard.ChangeValue(bonus);
        }

        // 설명에는 최대값을 표시 ({0} = max)
        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { maxValue.Get(grade) };
    }
}
