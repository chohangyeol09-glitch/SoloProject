namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class BurnCostEvent : GameEvent
    {
        public int Amount;

        public BurnCostEvent Init(int amount)
        {
            Amount = amount;
            return this;
        }
    }
}
