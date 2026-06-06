using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using UnityEngine;

namespace _02.Scripts.SlotSystem.Slots
{
    public class EnemySlot : AbstractSlot
    {
        [SerializeField] private EventChannelSO slotChannel;

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            slotChannel.AddListener<CardPickUpEvent>(HandleCardPickedUp);
        }

        private void OnDestroy()
        {
            slotChannel.RemoveListener<CardPickUpEvent>(HandleCardPickedUp);
        }

        private void HandleCardPickedUp(CardPickUpEvent evt)
        {
            if (CurrentCard != evt.Card) return;
            RemoveCurrentCard();
        }
    }
}