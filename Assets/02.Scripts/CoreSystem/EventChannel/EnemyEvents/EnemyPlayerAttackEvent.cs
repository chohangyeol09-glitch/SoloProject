namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemyPlayerAttackEvent : GameEvent
    {
        public int Value { get; private set; }

        public void Init(int value)
        {
            Value = value;
        }
    }
}