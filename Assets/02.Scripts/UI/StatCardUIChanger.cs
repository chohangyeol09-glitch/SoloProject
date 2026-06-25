using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.ModuleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.UI
{
    public class StatCardUIChanger : MonoBehaviour, IModule
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image mainIcon;
        [SerializeField] private Image gradeOutline;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI valueText;
        
        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void SetUI(StatCardDataSO data)
        {
            nameText.text = data.Name;
            descriptionText.text = data.GetDescription();
            mainIcon.sprite = data.Icon;
            costText.text = data.Cost.ToString();
            if (valueText != null)
                valueText.text = data.Value.ToString();
            gradeOutline.color = data.Grade switch
            {
                CardGrade.BRONZE => Color.saddleBrown,
                CardGrade.SILVER => Color.silver,
                CardGrade.GOLD => Color.gold,
                _ => Color.white
            };
        }
    }
}