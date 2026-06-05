namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class TurnStartEvent : GameEvent
    {
        public int CurrentTurn { get; private set; }
        
        public TurnStartEvent Init(int currentTurn)
        {
            CurrentTurn = currentTurn;
            return this;
        }
        
    }
}