using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    public abstract class AbstractActionEffectSO : ScriptableObject
    {
        public abstract bool IsActivate(EffectExecuteContext context);
        
        public abstract void Apply(EffectExecuteContext context, List<Slot> targets);
    }
}