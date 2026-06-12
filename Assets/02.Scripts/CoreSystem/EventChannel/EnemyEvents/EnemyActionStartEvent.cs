using System;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemyActionStartEvent : GameEvent
    {
        public Action OnComplete;

        public EnemyActionStartEvent Init(Action onComplete)
        {
            OnComplete = onComplete;
            return this;
        }
    }
}