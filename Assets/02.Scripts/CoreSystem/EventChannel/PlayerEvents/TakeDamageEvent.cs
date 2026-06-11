using System;

namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class TakeDamageEvent : GameEvent
    {
        public int Value;
        public Action<bool> OnDead; 

        public TakeDamageEvent Init(int amount, Action<bool> onDead = null)
        {
            Value = amount;
            OnDead = onDead;
            return this;
        }
    }
}