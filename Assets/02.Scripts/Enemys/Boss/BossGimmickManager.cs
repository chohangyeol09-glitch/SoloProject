using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public class BossGimmickManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private SlotLogic slotLogic;
        
        private EnemyDataSO _currentEnemyData;
        private int _currentTurn;
        private readonly HashSet<AbstractBossConditionSO> _triggeredOnceConditions = new();

        private void Awake()
        {
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            enemyEventChannel.AddListener<EnemyActionStartEvent>(HandleEnemyActionStart);
            enemyEventChannel.AddListener<EnemyActionEndEvent>(HandleEnemyActionEnd);
            turnEventChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            enemyEventChannel.RemoveListener<EnemyActionStartEvent>(HandleEnemyActionStart);
            enemyEventChannel.RemoveListener<EnemyActionEndEvent>(HandleEnemyActionEnd);
            turnEventChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            _currentEnemyData = evt.EnemyData;
            _currentTurn = 0;
            _triggeredOnceConditions.Clear();
        }

        private void HandleTurnChange(TurnChangeEvent evt) => _currentTurn = evt.CurrentTurn;

        private void HandleEnemyActionStart(EnemyActionStartEvent evt)
            => ExecuteGimmicks(BossGimmickTiming.OnEnemyActionStart, _currentTurn, evt.OnComplete);

        private void HandleEnemyActionEnd(EnemyActionEndEvent evt)
            => ExecuteGimmicks(BossGimmickTiming.OnEnemyActionEnd, _currentTurn, evt.OnComplete);

        public bool HasGimmick(BossGimmickTiming timing)
        {
            Debug.Log($"HasGimmick: {timing}");
            if (_currentEnemyData == null || !_currentEnemyData.IsBoss) return false;
            BossGimmickContext context = CreateContext();
            foreach (BossGimmick gimmick in _currentEnemyData.BossGimmicks)
            {
                if (gimmick.Timing != timing) continue;
                if (gimmick.Condition != null && !gimmick.Condition.IsActivate(context)) continue;
                if (gimmick.Condition != null && gimmick.Condition.TriggerOnce && _triggeredOnceConditions.Contains(gimmick.Condition)) continue;
                return true;
            }
            return false;
        }

        public void ExecuteGimmicks(BossGimmickTiming timing, int currentTurn, Action onComplete)
        {
            if (_currentEnemyData == null || !_currentEnemyData.IsBoss)
            {
                onComplete?.Invoke();
                return;
            }

            List<AbstractBossPatternSO> patterns = new List<AbstractBossPatternSO>();
            BossGimmickContext context = CreateContext();
            context.CurrentTurn = currentTurn;

            foreach (BossGimmick gimmick in _currentEnemyData.BossGimmicks)
            {
                if (gimmick.Timing != timing) continue;
                if (gimmick.Condition != null && !gimmick.Condition.IsActivate(context)) continue;
                if (gimmick.Condition != null && gimmick.Condition.TriggerOnce)
                {
                    if (_triggeredOnceConditions.Contains(gimmick.Condition)) continue;
                    _triggeredOnceConditions.Add(gimmick.Condition);
                }
                if (gimmick.Pattern != null)
                    patterns.Add(gimmick.Pattern);
            }

            if (patterns.Count == 0) { onComplete?.Invoke(); return; }

            StartCoroutine(ExecutePatternsInOrder(patterns, context, onComplete));
        }

        private IEnumerator ExecutePatternsInOrder(List<AbstractBossPatternSO> patterns, BossGimmickContext context, Action onComplete)
        {
            foreach (AbstractBossPatternSO pattern in patterns)
            {
                bool done = false;
                pattern.Execute(context, () => done = true);
                yield return new WaitUntil(() => done);
            }
            onComplete?.Invoke();
        }

        private BossGimmickContext CreateContext() => new BossGimmickContext
        {
            CurrentTurn =  _currentTurn,
            CurrentHealth = Enemy.Instance.CurrentHealth,
            MaxHealth = Enemy.Instance.MaxHealth,
            SlotLogic = slotLogic
        };
    }
}