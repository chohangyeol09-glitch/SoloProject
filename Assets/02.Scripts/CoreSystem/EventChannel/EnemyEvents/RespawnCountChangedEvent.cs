namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class RespawnCountChangedEvent : GameEvent
    {
        public int Remaining { get; private set; }
        public int Max { get; private set; }

        public RespawnCountChangedEvent Init(int remaining, int max)
        {
            Remaining = remaining;
            Max = max;
            return this;
        }
    }
}
