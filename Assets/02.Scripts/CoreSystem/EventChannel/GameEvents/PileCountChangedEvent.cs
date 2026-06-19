namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class PileCountChangedEvent : GameEvent
    {
        public int DeckCount;
        public int DiscardCount;

        public PileCountChangedEvent Init(int deckCount, int discardCount)
        {
            DeckCount    = deckCount;
            DiscardCount = discardCount;
            return this;
        }
    }
}