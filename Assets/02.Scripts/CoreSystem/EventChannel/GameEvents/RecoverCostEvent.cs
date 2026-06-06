namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class RecoverCostEvent : GameEvent
    {
        public int? OverrideAmount;

        public RecoverCostEvent Init(int? overrideAmount = null)
        {
            OverrideAmount = overrideAmount;
            return this;
        }
    }
}