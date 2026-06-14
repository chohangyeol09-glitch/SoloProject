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

        public ActionEffectCardUIChanger UIChanger { get; private set; }
        private Vector3 _originPos;
        private Quaternion _originRot;

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            UIChanger = GetModule<ActionEffectCardUIChanger>();

            if (effect != null)
                UIChanger?.SetUI(effect);
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

        public void SetEffect(AbstractActionEffectSO effectSO)
        {
            effect = effectSO;
            UIChanger?.SetUI(effect);
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
            Destroy(gameObject);
        }
    }
}
