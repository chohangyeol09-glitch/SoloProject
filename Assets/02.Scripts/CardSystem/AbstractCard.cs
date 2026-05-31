using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.CardSystem
{
    public abstract class AbstractCard : ModuleOwner
    {
        public CardInteraction CardInteraction { get; private set; }
        protected override void InitializeModules()
        {
            base.InitializeModules();
            CardInteraction = GetModule<CardInteraction>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }
    }
}
