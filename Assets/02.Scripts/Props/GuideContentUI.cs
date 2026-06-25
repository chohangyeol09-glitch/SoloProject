using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Props
{
    public class GuideContentUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI descriptionText;
        public void SetUI(AbstractActionEffectSO data)
        {
            nameText.text = data.Name;
            icon.sprite = data.Icon;
            descriptionText.text = data.GetDescription(data.Grade);
        }
    }
}