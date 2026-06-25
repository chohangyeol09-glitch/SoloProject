using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
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
        [SerializeField] private List<ActionCardDataSO> bronzeActionCards = new();

        [Header("Silver")]
        [SerializeField] private List<StatCardDataSO> silverStatCards = new();
        [SerializeField] private List<ActionCardDataSO> silverActionCards = new();

        [Header("Gold")]
        [SerializeField] private List<StatCardDataSO> goldStatCards = new();
        [SerializeField] private List<ActionCardDataSO> goldActionCards = new();

        // 효과는 등급 구분 없이 한 리스트. 등급은 보상 시점에 효과카드로 운반된다.
        [Header("Effects (전 등급 공통)")]
        [SerializeField] private List<AbstractActionEffectSO> actionEffects = new();

        public IReadOnlyList<StatCardDataSO> GetStatCardsByGrade(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronzeStatCards,
            CardGrade.SILVER => silverStatCards,
            CardGrade.GOLD   => goldStatCards,
            _                => new List<StatCardDataSO>()
        };

        public IReadOnlyList<AbstractActionEffectSO> GetEffects() => actionEffects;

        public IReadOnlyList<ActionCardDataSO> GetActionCardsByGrade(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronzeActionCards,
            CardGrade.SILVER => silverActionCards,
            CardGrade.GOLD   => goldActionCards,
            _                => new List<ActionCardDataSO>()
        };
    }
}
