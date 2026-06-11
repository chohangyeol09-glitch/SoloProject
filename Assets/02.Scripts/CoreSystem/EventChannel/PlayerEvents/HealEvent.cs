namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class HealEvent : GameEvent
    {
        public int Amount;

        public HealEvent Init(int amount)
        {
            Amount = amount;
            return this;
        }
    }
}