using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    [CreateAssetMenu(fileName = "CardGradePool", menuName = "Card/CardGradePool", order = 0)]
    public class CardGradePoolSO : ScriptableObject
    {
        [Header("Bronze")]
        [SerializeField] private List<StatCardDataSO> bronzeStatCards = new();
        [SerializeField] private List<AbstractActionEffectSO> bronzeEffects = new();

        [Header("Silver")]
        [SerializeField] private List<StatCardDataSO> silverStatCards = new();
        [SerializeField] private List<AbstractActionEffectSO> silverEffects = new();

        [Header("Gold")]
        [SerializeField] private List<StatCardDataSO> goldStatCards = new();
        [SerializeField] private List<AbstractActionEffectSO> goldEffects = new();

        public IReadOnlyList<StatCardDataSO> GetStatCardsByGrade(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronzeStatCards,
            CardGrade.SILVER => silverStatCards,
            CardGrade.GOLD   => goldStatCards,
            _                => new List<StatCardDataSO>()
        };

        public IReadOnlyList<AbstractActionEffectSO> GetEffectsByGrade(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronzeEffects,
            CardGrade.SILVER => silverEffects,
            CardGrade.GOLD   => goldEffects,
            _                => new List<AbstractActionEffectSO>()
        };
    }
}
