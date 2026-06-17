using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class ActionSpeedChangeProp : ModuleOwner
    {
        public PropInteraction PropInteraction;

        [SerializeField] private float value;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            PropInteraction.OnClick += HandleClick;
        }

        private void HandleClick()
        {
            PresentationControl.AddSpeed(value);
        }
    }
}