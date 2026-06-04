using System;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CardSystem.Cards.StatCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.PlayerEvent;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class ActionCard : AbstractCard, IDropTarget
    {
        public int AttackValue
        {
            get => _attackValue;
            set
            {
                _attackValue = value;
                OnAttackValueChanged?.Invoke(_attackValue);
            }
        }
        public int DefenseValue
        {
            get => _defenseValue;
            set
            {
                _defenseValue = value;
                Debug.LogWarning($"{gameObject.name}: Defense Value: {_defenseValue}");
                OnDefenseValueChanged?.Invoke(_defenseValue);
            }
        }
        public DropInteraction DropInteraction { get; private set; }
        public ActionCardUISetter UiSetter { get; private set; }
        [field: SerializeField] public ActionCardDataSO ActionCardData { get; private set; }
        public event Action<bool> OnDropSuccess;
        public event Action<int> OnAttackValueChanged;
        public event Action<int> OnDefenseValueChanged;

        [SerializeField] private EventChannelSO playerChannel;
        
        private int _attackValue = 0;
        private int _defenseValue = 0;
        private bool _isDragging = false;
        private Vector3 _originPos;
        private bool _dragReady = false;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            DropInteraction = GetModule<DropInteraction>();
            UiSetter = GetModule<ActionCardUISetter>();
            
            Debug.Assert(DropInteraction != null, $"DropInteraction is null: {gameObject.name}");
            Debug.Assert(UiSetter != null, $"UiSetter is null: {gameObject.name}");
            
            DropInteraction.OnDropped -= HandleDrop;
            DropInteraction.OnDropped += HandleDrop;
            DropInteraction.SetCanDropType(typeof(StatCard));
            
            CardInteraction.OnDragStarted += HandleDragStarted;
            CardInteraction.OnDragUpdated += HandleDragUpdated;
            CardInteraction.OnDragEnded += HandleDragEnded;
            CardInteraction.OnHoverEntered += HandleHoverEntered;
            CardInteraction.OnHoverExited += HandleHoverExited;
            
            if (ActionCardData !=  null)
                SetData();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            OnAttackValueChanged?.Invoke(_attackValue);
            OnDefenseValueChanged?.Invoke(_defenseValue);
        }

        public void SetData()
        {
            //Icon.sprite = ActionCardData.Icon;
        }
        
        public void ChangeValue(int value)
        {
            if (ActionCardData.ActionCardType == ActionSO.ActionCardType.Attack)
                AttackValue += value;
            else if (ActionCardData.ActionCardType == ActionSO.ActionCardType.Defense)
                DefenseValue += value;
        }
        

        public void TakeDamage(int value)
        {
            int overflow = value - DefenseValue;
            DefenseValue = Mathf.Max(DefenseValue - value, 0);

            if (overflow > 0)
                playerChannel.RaiseEvent(new TakeDamageEvent().Init(overflow));
        }

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
        }
        
        #region Handles

        private void HandleDragStarted()
        {
            _originPos = transform.position; // ← 원래 위치 저장
            _dragReady = false;
            transform.DOKill();
            transform.DOMove(_originPos + Vector3.up * 1.5f, 0.15f)
                .OnComplete(() => _dragReady = true);
        }

        private void HandleDragUpdated(Vector3 pos)
        {
            if (!_dragReady) return;
            transform.position = pos;
        }

        private void HandleDragEnded()
        {
            _dragReady = false;
            transform.DOKill();
            transform.DOMove(_originPos, 0.3f); 
        }

        private void HandleHoverEntered()
        {
            /*Vector3 basePos = _handLogic.GetCardPosition(index, count);
            float targetY = (index + 2) * _handLogic.GetYPerIndex();
            basePos.y = targetY;
            transform.DOKill();
            transform.DOMove(basePos, 0.15f);*/
        }

        private void HandleHoverExited()
        {
            /*if (_handLogic == null) return;
            int index = _handLogic.GetCardIndex(this);
            int count = _handLogic.GetHandCount();
            transform.DOKill();
            transform.DOMove(_handLogic.GetCardPosition(index, count), 0.15f);*/
        }
        
        #endregion
        
        [ContextMenu("kte")]
        private void test() => Debug.LogWarning(DefenseValue);
        
        [ContextMenu("AddValue")]
        private void AddValue() => ChangeValue(10);
    }
}
