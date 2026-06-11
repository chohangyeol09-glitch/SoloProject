using System;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemyAttackStartEvent : GameEvent
    {
        public int TotalDamage;
        public Action OnAttackEnd;

        public EnemyAttackStartEvent Init(int totalDamage, Action onAttackEnd)
        {
            TotalDamage = totalDamage;
            OnAttackEnd = onAttackEnd;
            return this;
        }
    }
}