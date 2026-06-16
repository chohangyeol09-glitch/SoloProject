using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.SlotSystem.Slots;
using DG.Tweening;
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
            if (SlotText != null)
                SlotText.text = (SlotNumber+1).ToString();
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

            Vector3 pos = transform.position + Vector3.up * 3f;
            card.transform.position = pos;
            card.transform.DOMoveY(transform.position.y + 0.2f, 0.3f);
        }

        public void RemoveCurrentCard()
        {
            CurrentCard = null;
        }

        public void RegisterCard(ActionCard card)
        {
            if (CurrentCard != null)
                RemoveCurrentCard();
            CurrentCard = card;
            if (card is PlayerActionCard playerCard)
                playerCard.SetOriginalSlot(this as PlayerSlot);
        }

    }
}