using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    public abstract class AbstractActionEffectSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        public abstract bool IsActivate(EffectExecuteContext context);
        
        public abstract void Apply(EffectExecuteContext context, List<AbstractSlot> targets);
    }
}