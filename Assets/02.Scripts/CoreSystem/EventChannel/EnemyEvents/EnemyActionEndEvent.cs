using System;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemyActionEndEvent : GameEvent
    {
        public Action OnComplete;

        public EnemyActionEndEvent Init(Action onComplete)
        {
            OnComplete = onComplete;
            return this;
        }
    }
}