using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.SlotSystem.Slots;
using TMPro;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public abstract class AbstractSlot : ModuleOwner
    {
        public ActionCard CurrentCard { get; protected set; }

        [field: SerializeField] public SlotType SlotType { get; private set; }
        [field: SerializeField] public int SlotNumber { get; private set; }
        [field: SerializeField] public TextMeshPro SlotText { get; private set; }
        protected override void InitializeModules()
        {
            base.InitializeModules();
            SlotText.text = SlotNumber.ToString();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }

        public void SetCurrentCard(ActionCard card)
        {
            if (CurrentCard != null)
                RemoveCurrentCard();

            CurrentCard = card;

            if (card is PlayerActionCard playerCard)
                playerCard.SetOriginalSlot(this as PlayerSlot);

            Vector3 pos = transform.position;
            pos.y += 0.1f;
            card.transform.position = pos;
        }

        public void RemoveCurrentCard()
        {
            CurrentCard = null;
        }
        
    }
}