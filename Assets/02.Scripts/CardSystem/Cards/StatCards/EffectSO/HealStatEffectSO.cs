using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "HealEffect", menuName = "Card/Stat/Effect/Heal", order = 0)]
    public class HealStatEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private GradeValue healAmount;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { healAmount.Get(grade) };

        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            playerChannel.RaiseEvent(new HealEvent().Init(healAmount.Get(GetCardGrade(context))));
        }
    }
}
