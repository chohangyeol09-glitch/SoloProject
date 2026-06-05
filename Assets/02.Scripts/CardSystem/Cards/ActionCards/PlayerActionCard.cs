using System;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.InteractionSystem;
using _02.Scripts.InteractionSystem.Interactions;
using _02.Scripts.SlotSystem.Slots;
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
        private Outlinable _outline;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
            UIChanger = GetModule<ActionCardUIChanger>();
            _outline = GetComponent<Outlinable>();

            Debug.Assert(DropInteraction != null, $"DropInteraction is null: {gameObject.name}");
            Debug.Assert(UIChanger != null, $"UIChanger is null: {gameObject.name}");

            _outline.enabled = false;
            DropInteraction.OnDrop -= HandleDrop;
            DropInteraction.OnDrop += HandleDrop;
            DropInteraction.OnHoverEnter += HandleDropHoverEnter;
            DropInteraction.OnHoverExit += HandleDropHoverExit;
            DropInteraction.SetCanDropType(typeof(StatCard));
            if (ActionCardData != null) SetData();
        }

        private void OnDestroy()
        {
            if (DropInteraction != null)
            {
                DropInteraction.OnDrop -= HandleDrop;
                DropInteraction.OnHoverEnter -= HandleDropHoverEnter; 
                DropInteraction.OnHoverExit -= HandleDropHoverExit;
            }
        }

        public void SetData() { }

        public void HandleDrop(Transform dropTrm)
        {
            if (!dropTrm.TryGetComponent<StatCard>(out StatCard statCard))
            {
                OnDropSuccess?.Invoke(false);
                return;
            }

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
                    ChangeValue(statCard.StatData.Value);

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

        protected override void HandleDragStart(Vector3 mouseWorldPos)
        {
            slotChannel?.RaiseEvent(new CardPickUpEvent().Init(this));
            base.HandleDragStart(mouseWorldPos);
        }

        protected override void HandleHoverEnter()
        {
            base.HandleHoverEnter();
            _outline.enabled = true;
            _outline.OutlineParameters.Color = Color.white;
        }

        protected override void HandleHoverExit()
        {
            base.HandleHoverExit();
            _outline.enabled = false;
            _outline.OutlineParameters.Color = Color.clear;
        }
        private void HandleDropHoverEnter()
        {
            _outline.enabled = true;
            _outline.OutlineParameters.Color = Color.white; 
        }

        private void HandleDropHoverExit()
        {
            _outline.enabled = false;
            _outline.OutlineParameters.Color = Color.clear;
        }
        public void SetOriginalSlot(PlayerSlot slot) => OriginalSlot = slot;
        public void ForceReturn() => HandleDragEnd();

    }
}