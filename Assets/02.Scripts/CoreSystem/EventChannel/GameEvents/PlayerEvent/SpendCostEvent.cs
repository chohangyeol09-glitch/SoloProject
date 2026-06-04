using System;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.PlayerEvent
{
    public class SpendCostEvent : GameEvent
    {
        public int Amount;
        public Action<bool> OnResult;

        public SpendCostEvent Init(int amount, Action<bool> onResult)
        {
            Amount = amount;
            OnResult = onResult;
            return this;
        }
    }
}