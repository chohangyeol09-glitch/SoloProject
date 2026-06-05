
using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents
{
    public class CardPickUpEvent : GameEvent
    {
        public PlayerActionCard Card { get; private set; }
        
        public CardPickUpEvent Init(PlayerActionCard card)
        {
            Card = card;
            return this;
        }
    }
}