using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.InteractionSystem;
using _02.Scripts.InteractionSystem.Interactions;
using _02.Scripts.SlotSystem;
using _02.Scripts.SlotSystem.Slots;
using _02.Scripts.UI;
using EPOOutline;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class PlayerActionCard : ActionCard, IDropTarget
    {
        public DropInteraction DropInteraction { get; private set; }
        public ActionCardUIChanger UIChanger { get; private set; }
        public PlayerSlot OriginalSlot { get; private set; }
        public event Action<bool> OnDropSuccess;

        [SerializeField] private EventChannelSO slotChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        private Outlinable _outline;
        private SlotLogic _slotLogic;
        private SlotLogic SlotLogicCached => _slotLogic != null ? _slotLogic : (_slotLogic = FindFirstObjectByType<SlotLogic>());

        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
            UIChanger = GetModule<ActionCardUIChanger>();
            _outline = GetComponent<Outlinable>();

            Debug.Assert(DropInteraction != null, $"DropInteraction is null: {gameObject.name}");
            Debug.Assert(UIChanger != null, $"UIChanger is null: {gameObject.name}");

            //_outline.enabled = false;
            _outline.FrontParameters.Color = Color.clear;
            DropInteraction.OnDrop -= HandleDrop;
            DropInteraction.OnDrop += HandleDrop;
            DropInteraction.OnHoverEnter += HandleDropHoverEnter;
            DropInteraction.OnHoverExit += HandleDropHoverExit;
            DropInteraction.SetCanDropType(typeof(StatCard));
            DropInteraction.AddCanDropType(typeof(ActionEffectCard));
            if (ActionCardData != null) SetData();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            turnEventChannel.AddListener<TurnChangeEvent>(HandleTurnStart);
        }

        

        private void OnDestroy()
        {
            if (DropInteraction != null)
            {
                DropInteraction.OnDrop -= HandleDrop;
                DropInteraction.OnHoverEnter -= HandleDropHoverEnter; 
                DropInteraction.OnHoverExit -= HandleDropHoverExit;
            }
            
            turnEventChannel.RemoveListener<TurnChangeEvent>(HandleTurnStart);
        }

        public void SetData() { }

        protected override void OnDataSet()
        {
            base.OnDataSet();
            UIChanger?.SetUI(ActionCardData);
        }

        public void HandleDrop(Transform dropTrm)
        {
            if (dropTrm.TryGetComponent<StatCard>(out StatCard statCard))
            {
                HandleStatCardDrop(statCard);
                return;
            }

            if (dropTrm.TryGetComponent<ActionEffectCard>(out ActionEffectCard effectCard))
            {
                HandleEffectCardDrop(effectCard);
                return;
            }

            OnDropSuccess?.Invoke(false);
        }

        private void HandleStatCardDrop(StatCard statCard)
        {
            playerChannel.RaiseEvent(new SpendCostEvent().Init(
                statCard.NeedCost,
                success =>
                {
                    if (!success)
                    {
                        statCard.ReturnToOrigin();
                        OnDropSuccess?.Invoke(false);
                        return;
                    }

                    OnDropSuccess?.Invoke(true);
                    if (ActionCardData.ActionCardType == ActionCardType.Defense && OriginalSlot != null)
                    {
                        List<AbstractSlot> targets = SlotLogicCached.GetTargetSlots(OriginalSlot, ActionCardData.TargetRangeType);
                        if (ActionCardData.Action is DefenseActionSO defenseAction)
                            defenseAction.PlayGrant(this, targets, statCard.StatData.Value);
                        else
                            foreach (AbstractSlot target in targets)
                                target.CurrentCard?.AddDefenseValue(statCard.StatData.Value);
                    }
                    else
                    {
                        ChangeValue(statCard.StatData.Value);
                    }

                    foreach (AbstractStatEffectSO effect in statCard.StatData.Effects)
                    {
                        StatExecuteContext context = new StatExecuteContext(statCard, this);
                        if (effect.IsActivate(context))
                            effect.Apply(context);
                    }
                    statCard.OnUsed();
                }
            ));
            _outline.OutlineParameters.Color = Color.clear;
        }

        private void HandleEffectCardDrop(ActionEffectCard effectCard)
        {
            AddRuntimeEffect(effectCard.Effect);
            UIChanger.AddEffectIcon(effectCard.Effect);
            effectCard.OnUsed();
            OnDropSuccess?.Invoke(true);
            _outline.OutlineParameters.Color = Color.clear;
        }

        protected override void HandleDragStart(Vector3 mouseWorldPos)
        {
            slotChannel?.RaiseEvent(new CardPickUpEvent().Init(this));
            _outline.FrontParameters.Color = Color.clear;
            base.HandleDragStart(mouseWorldPos);
        }

        protected override void HandleDragEnd()
        {
            base.HandleDragEnd();
            OriginalSlot?.RegisterCard(this);
        }

        protected override void HandleHoverEnter()
        {
            base.HandleHoverEnter();
            _outline.FrontParameters.Color = Color.white;
        }

        protected override void HandleHoverExit()
        {
            base.HandleHoverExit();
            _outline.FrontParameters.Color = Color.clear;
        }
        private void HandleDropHoverEnter()
        {
            _outline.FrontParameters.Color = Color.white; 
        }
        
        private void HandleDropHoverExit()
        {
            _outline.FrontParameters.Color = Color.clear;
        }
        
        private void HandleTurnStart(TurnChangeEvent obj)
        {
            ResetValues();
        }
        
        public void SetOriginalSlot(PlayerSlot slot) => OriginalSlot = slot;
        public void ForceReturn() => HandleDragEnd();

    }
}