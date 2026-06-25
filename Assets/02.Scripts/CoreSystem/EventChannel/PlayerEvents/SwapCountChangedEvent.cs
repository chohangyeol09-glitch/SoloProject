namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class SwapCountChangedEvent : GameEvent
    {
        public int Remaining { get; private set; }
        public int Max { get; private set; }

        public SwapCountChangedEvent Init(int remaining, int max)
        {
            Remaining = remaining;
            Max = max;
            return this;
        }
    }
}
