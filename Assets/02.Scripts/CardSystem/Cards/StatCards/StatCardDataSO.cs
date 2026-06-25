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
        [SerializeField] private GradeValue value;
        public List<AbstractStatEffectSO> Effects = new();

        public int Value => value.Get(Grade);

        public void SetGrade(CardGrade grade) => Grade = grade;

        public string GetDescription()
        {
            List<object> args = new();
            foreach (AbstractStatEffectSO effect in Effects)
                if (effect != null)
                    args.AddRange(effect.GetDescriptionArgs(Grade));
            return args.Count > 0 ? string.Format(Description, args.ToArray()) : Description;
        }

        private void OnValidate()
        {
            Name = name;
        }
    }
}
