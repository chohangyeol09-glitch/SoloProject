using System.Collections.Generic;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    [CreateAssetMenu(fileName = "DefaultStatCardListSO", menuName = "Card/Stat/DefaultStatCardList", order = 0)]
    public class StartStatCardListSO : ScriptableObject
    {
        [field:SerializeField] public List<StatCardDataSO> StatCards { get; private set; }
    }
}