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

        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            
            PropInteraction.OnClick += HandleClick;
        }

        public void Show()
        {
            Debug.Log("Active");
            gameObject.SetActive(true);
        }

        private void HandleClick()
        {
            stageManager.StartNextStage();
        }
        
    }
}