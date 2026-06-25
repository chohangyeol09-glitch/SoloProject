using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemyCardDestroyedEvent : GameEvent
    {
        public EnemyActionCard Card { get; private set; }

        public EnemyCardDestroyedEvent Init(EnemyActionCard card)
        {
            Card = card;
            return this;
        }
    }
}
