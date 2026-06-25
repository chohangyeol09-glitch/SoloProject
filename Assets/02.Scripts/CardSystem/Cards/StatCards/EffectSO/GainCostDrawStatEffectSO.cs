using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.Players;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    // 코스트를 지불한 뒤 남은 코스트가 0이면 코스트를 회복하고 카드를 드로우한다.
    [CreateAssetMenu(fileName = "GainCostDraw", menuName = "Card/Stat/Effect/GainCostDraw", order = 0)]
    public class GainCostDrawStatEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private EventChannelSO playerChannel;
        [SerializeField] private EventChannelSO cardEventChannel;
        [SerializeField] private GradeValue gainCostAmount;
        [SerializeField] private GradeValue drawCount;

        public override object[] GetDescriptionArgs(CardGrade grade) => new object[] { gainCostAmount.Get(grade), drawCount.Get(grade) };

        public override bool IsActivate(StatExecuteContext context)
            => Player.Instance.CurrentCost == 0;

        public override void Apply(StatExecuteContext context)
        {
            CardGrade grade = GetCardGrade(context);
            playerChannel.RaiseEvent(new GainCostEvent().Init(gainCostAmount.Get(grade)));
            cardEventChannel.RaiseEvent(new DrawCardEvent().Init(drawCount.Get(grade)));
        }
    }
}
