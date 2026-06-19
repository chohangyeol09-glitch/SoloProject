using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02.Scripts.UI
{
    public class InfoShowProp : MonoBehaviour, IInfoShowable, IPointerEnterHandler, IPointerExitHandler, IModule
    {
        [SerializeField] private EventChannelSO eventChannel;

        private string _title;
        private string _description;

        public void SetInfo(string title, string description)
        {
            _title = title;
            _description = description;
        }

        public void ShowInfo()
        {
            eventChannel.RaiseEvent(new InfoShowEvent().Init(_title, _description));
        }

        public void HideInfo()
        {
            eventChannel.RaiseEvent(new InfoHideEvent());
        }

        public void OnPointerEnter(PointerEventData eventData) => ShowInfo();
        public void OnPointerExit(PointerEventData eventData) => HideInfo();
        public void Initialize(ModuleOwner owner)
        {
            
        }
    }
}