using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02.Scripts.Props
{
    public class InfoShowProp : MonoBehaviour, IInfoShowable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private EventChannelSO eventChannel;
        [SerializeField] private string defaultTitle;
        [TextArea]
        [SerializeField] private string defaultDescription;
        private string _title = "";
        private string _description = "";

        public void SetInfo(string title, string description)
        {
            _title = title;
            _description = description;
        }

        public void ShowInfo()
        {
            if (_title == "" || _description == "")
            {
                eventChannel.RaiseEvent(new InfoShowEvent().Init(defaultTitle, defaultDescription));
            }
            else
            {
                eventChannel.RaiseEvent(new InfoShowEvent().Init(_title, _description));
            }
        }

        public void HideInfo()
        {
            eventChannel.RaiseEvent(new InfoHideEvent());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowInfo();
        }

        public void OnPointerExit(PointerEventData eventData) => HideInfo();
    }
}