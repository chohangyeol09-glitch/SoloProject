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
        }

        protected override void AfterInitializeModules()
        {
            DropInteraction.OnDropped += OnDrop;
        }
        
        private void OnDestroy()
        {
            DropInteraction.OnDropped -= OnDrop;
        }
        
        public void OnDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<ActionCard>(out ActionCard card)) return;
            
            Vector3 pos = transform.position;
            pos.y += 0.1f;
            dropTrm.transform.position = pos;

            SetCurrentCard(card);
            OnDropCard?.Invoke(this, SlotNumber);
        }
    }
}