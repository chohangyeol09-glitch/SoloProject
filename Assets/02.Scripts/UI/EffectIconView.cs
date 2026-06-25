using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.Props;
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
        
        public void Setup(AbstractActionEffectSO effect, CardGrade grade)
        {
            icon.sprite = effect.Icon;
            bool show = effect.HasDisplayValue;
            valueText.gameObject.SetActive(show);
            if (show)
                valueText.text = effect.GetDisplayValue(grade).ToString();
            else
                valueText.text = "";

            infoShow.SetInfo(effect.Name, effect.GetDescription(grade));
        }
    }
}
