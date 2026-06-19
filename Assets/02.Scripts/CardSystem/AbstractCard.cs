using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    public abstract class AbstractCard : ModuleOwner
    {
        public CardInteraction CardInteraction { get; private set; }
        public bool IsUpDownMoving { get; private set; }
        [SerializeField] protected LayerMask dropLayer;

        protected Vector3 OriginPos { get; private set; }
        protected Quaternion OriginRot { get; private set; }
        private bool DragReady { get; set; }
        

        [SerializeField] private float dragUpOffset = 1.5f;
        [SerializeField] private float dragUpDuration = 0.15f;
        private Quaternion _defaultRotation;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            _defaultRotation = transform.rotation;
            CardInteraction = GetModule<CardInteraction>();
            if (CardInteraction == null) return;
            CardInteraction.DropLayer = dropLayer;
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            if (CardInteraction == null) return;
            
            CardInteraction.OnDragStart += HandleDragStart;
            CardInteraction.OnDragUpdate += HandleDragUpdate;
            CardInteraction.OnDragEnd += HandleDragEnd;
            CardInteraction.OnHoverEnter += HandleHoverEnter;
            CardInteraction.OnHoverExit += HandleHoverExit;
        }

        protected virtual void OnDestroy()
        {
            if (CardInteraction == null) return;
            
            CardInteraction.OnDragStart -= HandleDragStart;
            CardInteraction.OnDragUpdate -= HandleDragUpdate;
            CardInteraction.OnDragEnd -= HandleDragEnd;
            CardInteraction.OnHoverEnter -= HandleHoverEnter;
            CardInteraction.OnHoverExit -= HandleHoverExit;
            transform.DOKill();
        }
        
        protected virtual void HandleDragStart(Vector3 mouseWorldPos)
        {
            if (IsUpDownMoving) return; 
    
            OriginPos = transform.position;
            OriginRot = transform.rotation;
            DragReady = false;
            IsUpDownMoving = true;
            transform.DOKill();

            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(mouseWorldPos, dragUpDuration));
            seq.Join(transform.DORotateQuaternion(_defaultRotation, dragUpDuration));
            seq.OnComplete(() =>
            {
                DragReady = true;
                IsUpDownMoving = false;
            });
        }

        protected virtual void HandleDragUpdate(Vector3 pos)
        {
            if (!DragReady) return;
            transform.position = pos;
        }
        
        protected virtual void HandleDragEnd()
        {
            DragReady = false;
            IsUpDownMoving = true;
            transform.DOKill();

            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(OriginPos, 0.3f));
            seq.Join(transform.DORotateQuaternion(OriginRot, 0.3f));
            seq.OnComplete(() => IsUpDownMoving = false);
        }

        // 슬롯/보관소로 내려놓는 모션. 이동 중엔 IsUpDownMoving=true라 드래그로 못 잡아챈다.
        public Tween PlayDropToSlot(Vector3 from, float targetY, float duration = 0.3f)
        {
            IsUpDownMoving = true;
            transform.DOKill();
            transform.position = from;
            return transform.DOMoveY(targetY, duration).OnComplete(() => IsUpDownMoving = false);
        }

        protected virtual void HandleHoverEnter() { }
        protected virtual void HandleHoverExit() { }
    }
}