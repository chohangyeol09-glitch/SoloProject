namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class CostChangedEvent : GameEvent
    {
        public int CurrentCost;

        public CostChangedEvent Init(int cost)
        {
            CurrentCost = cost; 
            return this;
        }
    }
}