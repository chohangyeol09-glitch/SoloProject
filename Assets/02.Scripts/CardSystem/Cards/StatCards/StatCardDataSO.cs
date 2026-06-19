using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    [CreateAssetMenu(fileName = "StatCardData", menuName = "Card/StatCardData")]
    public class StatCardDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public CardGrade Grade { get; private set; }
        [field: SerializeField]  public int Cost {get; private set;}
        [field: SerializeField] public int Value {get; private set;}
        public List<AbstractStatEffectSO> Effects = new();

        private void OnValidate()
        {
            Name = name;
        }
    }
}
