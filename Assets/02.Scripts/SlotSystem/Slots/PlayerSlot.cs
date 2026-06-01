using System;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.SlotSystem.Slots
{
    public class PlayerSlot : AbstractSlot
    {
        public event Action<AbstractSlot, int> OnDropCard; 
        public DropInteraction DropInteraction {get; private set;}

        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
            Debug.Assert(DropInteraction != null, "DropInteraction is null: " + gameObject.name);
            
            DropInteraction.SetCanDropType(typeof(ActionCard));
        }

        protected override void AfterInitializeModules()
        {
            DropInteraction.OnDropped += HandleDrop;
        }
        
        private void OnDestroy()
        {
            DropInteraction.OnDropped -= HandleDrop;
        }
        
        public void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<ActionCard>(out ActionCard card)) return;
            
            SetCurrentCard(card);
            OnDropCard?.Invoke(this, SlotNumber);
        }
    }
}