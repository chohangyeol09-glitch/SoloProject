namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.PlayerEvent
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