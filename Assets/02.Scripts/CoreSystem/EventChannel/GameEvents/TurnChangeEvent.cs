namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class TurnChangeEvent : GameEvent
    {
        public int CurrentTurn { get; private set; }
        
        public TurnChangeEvent Init(int currentTurn)
        {
            CurrentTurn = currentTurn;
            return this;
        }
        
    }
}