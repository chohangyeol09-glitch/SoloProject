using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "DrawCard", menuName = "Card/Stat/Effect/DrawCard", order = 0)]
    public class DrawCardEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private EventChannelSO cardEventChannel;
        [SerializeField] private GradeValue count;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { count.Get(grade) };
        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            cardEventChannel.RaiseEvent(new DrawCardEvent().Init(count.Get(GetCardGrade(context))));
        }
    }
}
