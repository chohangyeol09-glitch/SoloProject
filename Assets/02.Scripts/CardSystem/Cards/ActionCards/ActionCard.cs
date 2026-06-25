using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.Props;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public abstract class ActionCard : AbstractCard
    {
        
        public int AttackValue
        {
            get => _attackValue;
            private set
            {
                _attackValue = value;
                OnAttackValueChanged?.Invoke(_attackValue);
            }
        }
        public int DefenseValue
        {
            get => _defenseValue;
            private set
            {
                int prev = _defenseValue;
                _defenseValue = value;
                OnDefenseValueChanged?.Invoke(_defenseValue);
                if (prev > 0 && _defenseValue <= 0)
                    OnDefenseZero();
            }
        }

        [field: SerializeField] public ActionCardDataSO ActionCardData { get; private set; }
        public event Action<int> OnAttackValueChanged;
        public event Action<int> OnDefenseValueChanged;
        [SerializeField] private InfoShowProp targetTypeInfoProp;
        [SerializeField] private InfoShowProp cardTypeInfoProp;
        [SerializeField] protected EventChannelSO playerChannel;
        [SerializeField] protected EventChannelSO enemyChannel;
        
        [SerializeField] private float shakeStrength = 0.5f;
        [SerializeField] private float shakeDuration = 0.3f;

        private int _attackValue = 0;
        private int _defenseValue = 0;

        private readonly List<RuntimeActionEffect> _runtimeBeforeEffects = new();
        private readonly List<RuntimeActionEffect> _runtimeAfterEffects = new();

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            OnAttackValueChanged?.Invoke(_attackValue);
            OnDefenseValueChanged?.Invoke(_defenseValue);
        }

        private void UpdateInfoDisplay()
        {
            if (ActionCardData == null) return;

            string targetTitleS = "";
            string targetDescriptionS = "";
            switch (ActionCardData.TargetRangeType)  
            {
                case SlotTargetRangeType.FRONT_F:
                    targetTitleS = "전방";
                    targetDescriptionS = "카드의 반대편 칸을";
                    break;
                case SlotTargetRangeType.FRONT_LR:
                    targetTitleS = "전방_양옆";
                    targetDescriptionS = "카드의 전방칸의 양옆칸을";
                    break;
                case SlotTargetRangeType.FRONT_ONE:
                    targetTitleS = "적_첫번째";
                    targetDescriptionS = "적의 첫번째 슬롯을";
                    break;
                case  SlotTargetRangeType.FRONT_TWO:
                    targetTitleS = "적_두번째";
                    targetDescriptionS ="적의 두번째 슬롯을";
                    break;
                case SlotTargetRangeType.FRONT_THREE:
                    targetTitleS ="적_세번째";
                    targetDescriptionS ="적의 세번째 슬롯을";
                    break;
                case  SlotTargetRangeType.FRONT_FOUR:
                    targetTitleS = "적_네번째";
                    targetDescriptionS ="적의 네번째 슬롯을";
                    break;
                case SlotTargetRangeType.SELF:
                    targetTitleS = "자신";
                    targetDescriptionS = "카드 자신을";
                    break;
                case SlotTargetRangeType.LEFT:
                    targetTitleS = "왼쪽";
                    targetDescriptionS = "자신의 왼쪽을";
                    break;
                case SlotTargetRangeType.RIGHT:
                    targetTitleS = "오른쪽";
                    targetDescriptionS = "자신의 오른쪽을";
                    break;
                case SlotTargetRangeType.LR:
                    targetTitleS = "양옆";
                    targetDescriptionS = "자신의 양옆을";
                    break;
                case SlotTargetRangeType.RENDOM:
                    targetTitleS = "무작위";
                    targetDescriptionS = "무작위 적을";
                    break;
                case SlotTargetRangeType.ONE:
                    targetTitleS = "첫번째";
                    targetDescriptionS = "아군의 첫번째 슬롯을";
                    break;
                case SlotTargetRangeType.TWO:
                    targetTitleS = "두번째";
                        targetDescriptionS ="아군의 두번째 슬롯을";
                    break;
                case SlotTargetRangeType.THREE:
                    targetTitleS = "세번째";
                        targetDescriptionS ="아군의 세번째 슬롯을";
                    break;
                case SlotTargetRangeType.FOUR:
                    targetTitleS = "네번째";
                        targetDescriptionS ="아군의 네번째 슬롯을";
                    break;
                case SlotTargetRangeType.ALL:
                    targetTitleS = "적_전체";
                    targetDescriptionS = "적 전체를 ";
                    break;
                
            }
            targetDescriptionS += " 대상으로 합니다.";

            string cardTypeTitleS = "";
            string cardTypeDescriptionS = "";

            switch (ActionCardData.ActionCardType)
            {
                case ActionCardType.Attack:
                    cardTypeTitleS = "공격";
                    cardTypeDescriptionS = "적을 공격합니다. 값이 공격력으로 더해집니다.";
                    break;
                case ActionCardType.Defense:
                    cardTypeTitleS = " 방어";
                    cardTypeDescriptionS = "아군을 방어합니다. 값이 방어력으로 더해집니다.";
                    break;
            }
            
            if (targetTypeInfoProp != null)
                targetTypeInfoProp.SetInfo(targetTitleS, targetDescriptionS);
            if (cardTypeInfoProp != null)
                cardTypeInfoProp.SetInfo(cardTypeTitleS, cardTypeDescriptionS);
        }

        public void SetActionCardData(ActionCardDataSO data)
        {
            ActionCardData = data;
            OnDataSet();
        }

        
        public void ChangeValue(int value)
        {
            if (ActionCardData.ActionCardType == ActionCardType.Attack)
                AttackValue += value;
            else if (ActionCardData.ActionCardType == ActionCardType.Defense)
                DefenseValue += value;
        }

        public void AddAttackValue(int value) => AttackValue += value;
        public void AddDefenseValue(int value) => DefenseValue += value;

        public void ResetValues()
        {
            AttackValue = 0;
            DefenseValue = 0;
        }
        
        public void TakeDamage(int value, bool shake = true)
        {
            int overflow = value - DefenseValue;
            DefenseValue = Mathf.Max(DefenseValue - value, 0);

            if (shake)
            {
                Debug.Log("Shake");
                PlayHitShake(value);
            }

            if (overflow > 0)
            {
                if (this is PlayerActionCard)
                    playerChannel.RaiseEvent(new TakeDamageEvent().Init(overflow));
                else if (this is EnemyActionCard)
                    enemyChannel.RaiseEvent(new TakeDamageEvent().Init(overflow));
            }
        }
        
        public int GetOverflowDamage(int value)
        {
            int overflow = value - DefenseValue;
            DefenseValue = Mathf.Max(DefenseValue - value, 0);
            PlayHitShake(value);
            return Mathf.Max(overflow, 0);
        }

        protected virtual void OnDefenseZero() { }

        public void PlayHitShake(int value)
        {
            float strength = Mathf.Clamp(value * 0.05f, 0.1f, 0.5f);
            float duration = Mathf.Clamp(value * 0.03f, 0.2f, 0.5f);

            Vector3 originPos = transform.position;
            Vector3 shakeDir = transform.right * strength;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(originPos + shakeDir, duration / 4));
            seq.Append(transform.DOMove(originPos - shakeDir, duration / 2));
            seq.Append(transform.DOMove(originPos + shakeDir * 0.5f, duration / 4));
            seq.Append(transform.DOMove(originPos, duration / 4));
        }
        public void RaisePlayerDamage(int value)
        {
            playerChannel.RaiseEvent(new TakeDamageEvent().Init(value));
        }

        public void RaiseEnemyDamage(int value)
        {
            enemyChannel.RaiseEvent(new TakeDamageEvent().Init(value));
        }

        public const int MaxRuntimeEffects = 3;

        public int RuntimeEffectCount => _runtimeBeforeEffects.Count + _runtimeAfterEffects.Count;
        public bool CanAddRuntimeEffect => RuntimeEffectCount < MaxRuntimeEffects;

        protected bool AddRuntimeEffect(AbstractActionEffectSO effect, CardGrade grade)
        {
            if (!CanAddRuntimeEffect)
                return false;

            if (effect.Timing == EffectTimingType.Before)
                _runtimeBeforeEffects.Add(new RuntimeActionEffect(effect, grade));
            else
                _runtimeAfterEffects.Add(new RuntimeActionEffect(effect, grade));
            return true;
        }

        public IEnumerable<RuntimeActionEffect> GetBeforeEffects()
        {
            foreach (var e in ActionCardData.BeforeEffects) yield return new RuntimeActionEffect(e, e.Grade);
            foreach (var e in _runtimeBeforeEffects) yield return e;
        }

        public IEnumerable<RuntimeActionEffect> GetAfterEffects()
        {
            foreach (var e in ActionCardData.AfterEffects) yield return new RuntimeActionEffect(e, e.Grade);
            foreach (var e in _runtimeAfterEffects) yield return e;
        }

        protected virtual void OnDataSet()
        {
            OnAttackValueChanged?.Invoke(0);
            OnDefenseValueChanged?.Invoke(0);
            UpdateInfoDisplay();
        }
        
        
        [ContextMenu("Add AttackValue")]
        private void TestAddAttackValue() => AddAttackValue(10);
        [ContextMenu("Add DefenseValue")]
        private void TestAddDefenseValue() => AddDefenseValue(10);
    }
}
