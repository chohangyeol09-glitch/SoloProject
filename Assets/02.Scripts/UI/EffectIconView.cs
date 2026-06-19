using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.UI
{
    public class EffectIconView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private InfoShowProp infoShow;
        
        public void Setup(AbstractActionEffectSO effect)
        {
            icon.sprite = effect.Icon;
            bool show = effect.HasDisplayValue;
            valueText.gameObject.SetActive(show);
            if (show)
                valueText.text = effect.DisplayValue.ToString();
            else 
                valueText.text = "";
            
            infoShow.SetInfo(effect.Name, effect.Description);
        }
    }
}
