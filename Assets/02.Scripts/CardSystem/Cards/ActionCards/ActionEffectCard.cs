using System;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class ActionEffectCard : AbstractCard
    {
        [SerializeField] private AbstractActionEffectSO effect;
        public AbstractActionEffectSO Effect => effect;

        public event Action OnCardUsed;

        // 카드 선택 중(Busy 상태) 액션카드에 적용하기 위해 드래그가 허용돼야 한다.
        public override bool DraggableWhileBusy => true;

        public ActionEffectCardUIChanger UIChanger { get; private set; }
        public CardGrade Grade { get; private set; }
        private Vector3 _originPos;
        private Quaternion _originRot;

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            UIChanger = GetModule<ActionEffectCardUIChanger>();

            if (effect != null)
            {
                Grade = effect.Grade;
                UIChanger?.SetUI(effect, Grade);
            }
        }

        protected override void HandleDragStart(Vector3 mouseWorldPos)
        {
            _originPos = transform.position;
            _originRot = transform.rotation;
            base.HandleDragStart(mouseWorldPos);
        }

        protected override void HandleDragEnd()
        {
            transform.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(_originPos, 0.3f));
            seq.Join(transform.DORotateQuaternion(_originRot, 0.3f));
        }

        public void SetEffect(AbstractActionEffectSO effectSO, CardGrade grade)
        {
            effect = effectSO;
            Grade = grade;
            UIChanger?.SetUI(effect, Grade);
        }

        public void ReturnToOrigin()
        {
            transform.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Join(transform.DOMove(_originPos, 0.3f));
            seq.Join(transform.DORotateQuaternion(_originRot, 0.3f));
        }

        public void OnUsed()
        {
            transform.DOKill();
            OnCardUsed?.Invoke();
            Destroy(gameObject);
        }
    }
}
