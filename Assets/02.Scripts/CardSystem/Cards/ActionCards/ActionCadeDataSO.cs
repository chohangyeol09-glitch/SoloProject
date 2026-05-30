using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    [CreateAssetMenu(fileName = "ActionCadeData", menuName = "Card/ActionCardData", order = 0)]
    public class ActionCadeDataSO : ScriptableObject
    {
        [field: SerializeField] public SlotTargetRangeType TargetRangeType {get; private set;}
        [field: SerializeField] public AbstractActionSO Action {get; private set;}  
        [field: SerializeField] public List<AbstractActionEffectSO> Effects { get; private set; } = new();
    }
}