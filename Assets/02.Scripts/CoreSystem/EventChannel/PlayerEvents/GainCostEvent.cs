namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class GainCostEvent : GameEvent
    {
        public int Amount;

        public GainCostEvent Init(int amount)
        {
            Amount = amount;
            return this;
        }
    }
}