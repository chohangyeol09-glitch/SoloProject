using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using _02.Scripts.UI;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class ActionSpeedChangeProp : ModuleOwner
    {
        public PropInteraction PropInteraction { get; private set; }
        public InfoShowProp InfoShowProp {get; private set;}

        [SerializeField] private string infoTitle;
        [SerializeField] private string infoDescription;
        [SerializeField] private Color outLineColor;
        [SerializeField] private float value;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            InfoShowProp = GetModule<InfoShowProp>();
            PropInteraction.OnClick += HandleClick;
            PropInteraction.OnHoverEnter += HandleHoverEnter;
            PropInteraction.OnHoverExit += HandleHoverExit;
            
            InfoShowProp.SetInfo(infoTitle, infoDescription);
        }
        
        private void HandleClick()
        {
            PresentationControl.AddSpeed(value);
        }
        
        private void HandleHoverEnter()
        {
            PropInteraction.Outline.FrontParameters.Color = Color.clear;
            PropInteraction.Outline.BackParameters.Color = outLineColor;
        }

        private void HandleHoverExit()
        {
            PropInteraction.Outline.FrontParameters.Color = Color.clear;
            PropInteraction.Outline.BackParameters.Color = Color.clear;
        }

        
    }
}
