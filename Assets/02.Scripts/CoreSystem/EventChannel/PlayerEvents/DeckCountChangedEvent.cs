namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class DeckCountChangedEvent : GameEvent
    {
        public int CurrentCount;

        public DeckCountChangedEvent Init(int count)
        {
            CurrentCount = count;
            return this;
        }
    }
}
