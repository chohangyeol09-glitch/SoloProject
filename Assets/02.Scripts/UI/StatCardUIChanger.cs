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
        
        [Header("Sprite")]
        [SerializeField] public GradeSprite gradeSprite;

        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void SetUI(StatCardDataSO data)
        {
            nameText.text = data.Name;
            descriptionText.text = data.Description;
            mainIcon.sprite = data.Icon;
            costText.text = data.Cost.ToString();
            gradeOutline.sprite = data.Grade switch
            {
                CardGrade.BRONZE => gradeSprite.bronze,
                CardGrade.SILVER => gradeSprite.silver,
                CardGrade.GOLD => gradeSprite.gold,
                _ => null
            };
        }
    }
}