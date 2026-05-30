using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    public abstract class AbstractStatEffectSO : ScriptableObject
    {
        public abstract bool IsActivate(StatExecuteContext context);
        public abstract void Apply(StatExecuteContext context);
    }
}