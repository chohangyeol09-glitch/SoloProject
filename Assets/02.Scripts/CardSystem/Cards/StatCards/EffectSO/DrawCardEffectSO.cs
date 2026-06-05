using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    [CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card/Stat/Effect/DrawCard", order = 0)]
    public class DrawCardEffectSO : AbstractStatEffectSO
    {
        [SerializeField] private EventChannelSO cardEventChannel;
        [SerializeField] private int count;
        public override bool IsActivate(StatExecuteContext context) => true;

        public override void Apply(StatExecuteContext context)
        {
            DrawCardEvent drawCardEvent = new DrawCardEvent();
            drawCardEvent.Init(count);
            cardEventChannel.RaiseEvent(drawCardEvent);
        }
    }
}