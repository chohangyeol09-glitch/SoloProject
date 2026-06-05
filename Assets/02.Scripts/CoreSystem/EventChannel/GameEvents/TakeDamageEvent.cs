using System;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class TakeDamageEvent : GameEvent
    {
        public int Amount;
        public Action<bool> OnDead; 

        public TakeDamageEvent Init(int amount, Action<bool> onDead = null)
        {
            Amount = amount;
            OnDead = onDead;
            return this;
        }
    }
}