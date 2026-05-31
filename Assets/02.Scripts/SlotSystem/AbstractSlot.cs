using System;
using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public abstract class AbstractSlot : ModuleOwner
    {
        public ActionCard CurrentCard {get; protected set;}
        
        [field: SerializeField] public SlotType SlotType { get; private set; }
        [field: SerializeField] public int SlotNumber {get; private set;}

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }
        
        public void SetCurrentCard(ActionCard card)
        {
            CurrentCard = card;
        }



        
    }
}