using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards.EffectSO
{
    public abstract class AbstractStatEffectSO : ScriptableObject
    {
        [SerializeField] protected ActionCardType addValueType;
        public abstract bool IsActivate(StatExecuteContext context);
        public abstract void Apply(StatExecuteContext context);
    }
}