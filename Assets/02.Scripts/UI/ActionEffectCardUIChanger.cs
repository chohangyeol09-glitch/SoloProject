using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.ModuleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.UI
{
    class GradeSprite
    {
        public CardGrade grade;
        public Sprite sprite;
    }
    public class ActionEffectCardUIChanger : MonoBehaviour, IModule
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image mainIcon;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private GradeSprite gradeSprite;

        private ModuleOwner _owner;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void SetUI(AbstractActionEffectSO data)
        {
            nameText.text = data.Name;
            descriptionText.text = data.Description;
            mainIcon.sprite = data.Icon;

            bool show = data.HasDisplayValue;
            valueText.gameObject.SetActive(show);
            if (show)
                valueText.text = data.DisplayValue.ToString();
            
            
        }
    }
}
