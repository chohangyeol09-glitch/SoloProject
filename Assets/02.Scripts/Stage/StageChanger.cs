using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using EPOOutline;
using UnityEngine;

namespace _02.Scripts.Stage
{
    public class StageChanger : ModuleOwner
    {
        [SerializeField] private EventChannelSO gameChannel;
        [SerializeField] private StageManager stageManager;
        
        public PropInteraction PropInteraction { get; private set; }

        private Outlinable _outlinable;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            _outlinable = GetComponent<Outlinable>();
            
            PropInteraction.OnClick += HandleClick;
            PropInteraction.OnHoverEnter += HandleHoverEnter;
            PropInteraction.OnHoverExit += HandleHoverExit;
        }

        private void HandleClick()
        {
            _outlinable.FrontParameters.Color = Color.clear;
            stageManager.StartNextStage();
        }
        
        private void HandleHoverEnter()
        {
            _outlinable.FrontParameters.Color = Color.white;
        }
        
        private void HandleHoverExit()
        {
            _outlinable.FrontParameters.Color = Color.clear;
        }
        
    }
}