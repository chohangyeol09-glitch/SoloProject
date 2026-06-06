using System;

namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents
{
    public class ClearHandEvent : GameEvent
    {
        public Action OnComplete;

        public ClearHandEvent Init(Action onComplete)
        {
            OnComplete = onComplete;
            return this;
        }
    }
}