    using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using _02.Scripts.UI;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class LightInteract : ModuleOwner
    {
        [SerializeField] private GameObject lightObj;
        
        public PropInteraction PropInteraction {get; private set;}
        public InfoShowProp InfoShowProp {get; private set;}
        private bool _isLightOn = true;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            InfoShowProp = GetModule<InfoShowProp>();
            InfoShowProp.SetInfo("전등", "전기를 절약하자");
            PropInteraction.OnClick += HandleClick;
        }

        private void HandleClick()
        {
            if (_isLightOn)
            {
                lightObj.SetActive(false);
                _isLightOn = false;
            }
            else
            {
                lightObj.SetActive(true);
                _isLightOn = true;
            }
        }
    }
}