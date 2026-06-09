using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionWeapon;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    [CreateAssetMenu(fileName = "ActionCadeData", menuName = "Card/ActionCardData", order = 0)]
    public class ActionCardDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public  ActionCardType ActionCardType { get; private set; }
        [field: SerializeField] public SlotTargetRangeType TargetRangeType {get; private set;}
        [field: SerializeField] public AbstractActionSO Action {get; private set;}  
        [field: SerializeField] public List<AbstractActionEffectSO> BeforeEffects { get; private set; } = new();
        [field: SerializeField] public List<AbstractActionEffectSO> AfterEffects { get; private set; } = new();
        [field: SerializeField] public AbstractWeaponSO AbstractWeapon { get; private set; }
    }
}