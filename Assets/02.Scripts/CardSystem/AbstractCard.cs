using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.CardSystem
{
    public abstract class AbstractCard : ModuleOwner
    {
        public CardInteraction CardInteraction { get; private set; }
        
        [SerializeField] protected LayerMask dropLayer;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            CardInteraction = GetModule<CardInteraction>();
            CardInteraction.DropLayer = dropLayer;
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            
        }
    }
}
