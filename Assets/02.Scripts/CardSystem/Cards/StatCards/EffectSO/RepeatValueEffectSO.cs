using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "RepeatValueEffect", menuName = "Card/Stat/Effect/RepeatValue", order = 0)]
    public class RepeatValueEffectSO : AbstractStatEffectSO
    {
        [field: SerializeField] public int RepeatCount {get; private set;}
        [field: SerializeField] public int Value {get; private set;}

        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            Debug.Log("StatCardEffectApply");
            for (int i =  0; i < RepeatCount; i++)
                context.TargetActionCard.ChangeValue(Value);
        }
    }
}