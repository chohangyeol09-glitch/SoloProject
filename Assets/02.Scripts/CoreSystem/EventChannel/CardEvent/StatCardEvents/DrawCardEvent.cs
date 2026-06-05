namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvent
{
    public class DrawCardEvent : GameEvent
    {
        public int Count { get; private set; }

        public DrawCardEvent Init(int count)
        {
            Count = count;
            return this;
        }
    }
}