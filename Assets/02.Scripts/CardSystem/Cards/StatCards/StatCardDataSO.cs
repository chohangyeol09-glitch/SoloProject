using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    [CreateAssetMenu(fileName = "StatCardData", menuName = "Card/StatCardData")]
    public class StatCardDataSO : ScriptableObject
    { 
        [field: SerializeField]  public int Cost {get; private set;}
        [field: SerializeField] public int Value {get; private set;}
        public List<AbstractStatEffectSO> Effects = new();

    }
}
