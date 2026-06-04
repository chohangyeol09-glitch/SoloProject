using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.DackSystem;
using _02.Scripts.DeckSystem;
using _02.Scripts.Player;
using UnityEngine;

namespace _02.Scripts
{
    public class GameLogics : ModuleOwner
    {
        public DeckLogic DeckLogic { get; private set; }
        public CostLogic CostLogic { get; private set; }
        public HealthLogic HealthLogic { get; private set; }
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            DeckLogic = GetModule<DeckLogic>();
            CostLogic = GetModule<CostLogic>();
            HealthLogic = GetModule<HealthLogic>();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }
    }
}