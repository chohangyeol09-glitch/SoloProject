using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.DeckSystem;
using _02.Scripts.Players;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts
{
    public class GameManager : ModuleOwner
    {
        public SlotLogic SlotLogic { get; private set; }
        public BattleLogic BattleLogic { get; private set; }
        public HandLogic HandLogic { get; private set; }
        public DeckLogic DeckLogic { get; private set; }
        public EffectLogic EffectLogic { get; private set; }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            SlotLogic = GetModule<SlotLogic>();
            BattleLogic = GetModule<BattleLogic>();
            HandLogic = GetModule<HandLogic>();
            DeckLogic = GetModule<DeckLogic>();
            EffectLogic = GetModule<EffectLogic>();
            
            Debug.Assert(SlotLogic != null, "SlotLogic is null: " + gameObject.name);
            Debug.Assert(BattleLogic != null, "BattleLogic is null: " + gameObject.name);
            Debug.Assert(HandLogic != null, "HandLogic is null: " + gameObject.name);
            Debug.Assert(DeckLogic != null, "DeckLogic is null: " + gameObject.name);
            Debug.Assert(EffectLogic != null, "EffectLogic is null: " + gameObject.name);
        }
    }
}