using System;

namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents
{
    public class ExecuteCardsEvent : GameEvent
    {
        public Action OnComplete;

        public ExecuteCardsEvent Init(Action onComplete)
        {
            OnComplete = onComplete;
            return this;
        }
    }
}