using System;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents.BossEvents
{
    public class BossPatternStartEvent : GameEvent
    {
        public string AnimationName;
        public Action OnPatternEffect; 
        public Action OnPatternEnd;    

        public BossPatternStartEvent Init(string animName, Action onEffect, Action onEnd)
        {
            AnimationName = animName;
            OnPatternEffect = onEffect;
            OnPatternEnd = onEnd;
            return this;
        }
    }

}