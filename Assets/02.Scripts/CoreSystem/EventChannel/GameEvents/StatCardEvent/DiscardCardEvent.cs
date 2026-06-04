using _02.Scripts.CardSystem.Cards.StatCards;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.StatCardEvent
{
    public class DiscardCardEvent : GameEvent
    {
        public StatCardDataSO cardData;

        public DiscardCardEvent Init(StatCardDataSO cardData)
        {
            this.cardData = cardData;
            return this;
        }
    }
}