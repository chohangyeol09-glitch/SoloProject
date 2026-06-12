using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class TurnChangeProp : ModuleOwner
    {
        public PropInteraction PropInteraction {get; private set;}
        
        [SerializeField] private EventChannelSO turnEventChannel;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            PropInteraction.OnClick += HandleClick;
        }

        private void HandleClick()
        {
            Debug.Log("TurnEnd");
            turnEventChannel.RaiseEvent(new TurnEndEvent());
            
        }
    }
}
