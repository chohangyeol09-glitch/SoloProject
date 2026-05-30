using System;
using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.DragSystem;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class Slot : MonoBehaviour, IDropTarget, IHoverable
    {
        public ActionCard CurrentCard {get; private set;}
        
        public event Action<Slot, int> OnDropCard; 
        
        [field: SerializeField] public SlotType SlotType { get; private set; }
        [field: SerializeField] public int SlotNumber {get; private set;} 

        public void CardAction()
        {
            
        }
        
        public void OnHoverEnter()
        {
            
            Debug.Log("HoverEnter: " + gameObject.name);
        }

        public void OnHoverExit()
        {
            Debug.Log("HoverExit: " + gameObject.name);
        }

        public void OnDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<ActionCard>(out ActionCard card)) return;
            
            Vector3 pos = transform.position;
            pos.y += 0.3f;
            dropTrm.transform.position = pos;

            CurrentCard = card;
            OnDropCard?.Invoke(this, SlotNumber);
        }
    }
}