using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
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
                _defenseValue = value; 
                OnDefenseValueChanged?.Invoke(_defenseValue);
            }
        }

        [field: SerializeField] public ActionCardDataSO ActionCardData { get; private set; }
        public event Action<int> OnAttackValueChanged;
        public event Action<int> OnDefenseValueChanged;

        [SerializeField] protected EventChannelSO playerChannel;
        [SerializeField] protected EventChannelSO enemyChannel;
        
        [SerializeField] private float shakeStrength = 0.5f;
        [SerializeField] private float shakeDuration = 0.3f;

        private int _attackValue = 0;
        private int _defenseValue = 0;

        private readonly List<AbstractActionEffectSO> _runtimeBeforeEffects = new();
        private readonly List<AbstractActionEffectSO> _runtimeAfterEffects = new();

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            OnAttackValueChanged?.Invoke(_attackValue);
            OnDefenseValueChanged?.Invoke(_defenseValue);
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

            if (shake) PlayHitShake(value);

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
        
        public void AddRuntimeEffect(AbstractActionEffectSO effect)
        {
            if (effect.Timing == EffectTimingType.Before)
                _runtimeBeforeEffects.Add(effect);
            else
                _runtimeAfterEffects.Add(effect);
        }

        public IEnumerable<AbstractActionEffectSO> GetBeforeEffects()
        {
            foreach (var e in ActionCardData.BeforeEffects) yield return e;
            foreach (var e in _runtimeBeforeEffects) yield return e;
        }

        public IEnumerable<AbstractActionEffectSO> GetAfterEffects()
        {
            foreach (var e in ActionCardData.AfterEffects) yield return e;
            foreach (var e in _runtimeAfterEffects) yield return e;
        }

        protected virtual void OnDataSet()
        {
            OnAttackValueChanged?.Invoke(0);
            OnDefenseValueChanged?.Invoke(0);
        }
        
        
        [ContextMenu("Add AttackValue")]
        private void TestAddAttackValue() => AddAttackValue(10);
        [ContextMenu("Add DefenseValue")]
        private void TestAddDefenseValue() => AddDefenseValue(10);
    }
}
