
using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents
{
    public class CardPickUpEvent : GameEvent
    {
        public ActionCard Card {get; private set;}
        
        public CardPickUpEvent Init(ActionCard card)
        {
            Card = card;
            return this;
        }
    }
}