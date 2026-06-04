namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.PlayerEvent
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