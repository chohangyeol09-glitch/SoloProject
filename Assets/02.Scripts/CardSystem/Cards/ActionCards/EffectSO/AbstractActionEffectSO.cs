using System.Collections.Generic;
using _02.Scripts.CardSystem;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    public abstract class AbstractActionEffectSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public CardGrade Grade { get; private set; }
        [field: SerializeField] public EffectTimingType Timing { get; private set; }
        [field: SerializeField] public bool HasDisplayValue { get; private set; }
        [field: SerializeField] public int DisplayValue { get; private set; }
        public abstract bool IsActivate(EffectExecuteContext context);

        public abstract UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets);
    }
}