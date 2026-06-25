namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    // 런타임에 부착된 효과 + 그 효과에 적용할 등급.
    // 효과 SO는 상태를 갖지 않고(stateless), 등급을 이 쌍으로 함께 운반한다.
    public readonly struct RuntimeActionEffect
    {
        public readonly AbstractActionEffectSO Effect;
        public readonly CardGrade Grade;

        public RuntimeActionEffect(AbstractActionEffectSO effect, CardGrade grade)
        {
            Effect = effect;
            Grade = grade;
        }
    }
}
