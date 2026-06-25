using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public class EnemyPatternManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private SlotLogic slotLogic;
        
        private EnemyDataSO _currentEnemyData;
        private int _currentTurn;
        private readonly HashSet<AbstractEnemyConditionSO> _triggeredOnceConditions = new();

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

            if (_currentEnemyData != null)
                foreach (Gimmick gimmick in _currentEnemyData.Gimmicks)
                    gimmick.Condition?.ResetRuntimeState();
        }

        private void HandleTurnChange(TurnChangeEvent evt) => _currentTurn = evt.CurrentTurn;

        private void HandleEnemyActionStart(EnemyActionStartEvent evt)
            => ExecuteGimmicks(EnemyPatternTiming.OnEnemyActionStart, _currentTurn, evt.OnComplete);

        private void HandleEnemyActionEnd(EnemyActionEndEvent evt)
            => ExecuteGimmicks(EnemyPatternTiming.OnEnemyActionEnd, _currentTurn, evt.OnComplete);

        public bool HasGimmick(EnemyPatternTiming timing)
        {
            Debug.Log($"HasGimmick: {timing}");
            if (_currentEnemyData == null || !_currentEnemyData.IsBoss) return false;
            EnemyPatternContext context = CreateContext();
            foreach (Gimmick gimmick in _currentEnemyData.Gimmicks)
            {
                if (gimmick.Timing != timing) continue;
                if (gimmick.Condition != null && !gimmick.Condition.IsActivate(context)) continue;
                if (gimmick.Condition != null && gimmick.Condition.TriggerOnce && _triggeredOnceConditions.Contains(gimmick.Condition)) continue;
                return true;
            }
            return false;
        }

        public void ExecuteGimmicks(EnemyPatternTiming timing, int currentTurn, Action onComplete)
        {
            if (_currentEnemyData == null)
            {
                onComplete?.Invoke();
                return;
            }

            List<AbstractEnemyPatternSO> patterns = new List<AbstractEnemyPatternSO>();
            EnemyPatternContext context = CreateContext();
            context.CurrentTurn = currentTurn;

            foreach (Gimmick gimmick in _currentEnemyData.Gimmicks)
            {
                if (gimmick.Timing != timing) continue;
                if (gimmick.Condition != null && !gimmick.Condition.IsActivate(context)) continue;
                if (gimmick.Condition != null && gimmick.Condition.TriggerOnce)
                {
                    if (_triggeredOnceConditions.Contains(gimmick.Condition)) continue;
                    _triggeredOnceConditions.Add(gimmick.Condition);
                }
                gimmick.Condition?.NotifyTriggered(context);
                if (gimmick.Pattern != null)
                    patterns.Add(gimmick.Pattern);
            }

            if (patterns.Count == 0) { onComplete?.Invoke(); return; }

            RunPatterns(patterns, context, onComplete).Forget();
        }

        private async UniTaskVoid RunPatterns(List<AbstractEnemyPatternSO> patterns, EnemyPatternContext context, Action onComplete)
        {
            foreach (AbstractEnemyPatternSO pattern in patterns)
                await pattern.Execute(context);

            onComplete?.Invoke();
        }

        private EnemyPatternContext CreateContext() => new EnemyPatternContext
        {
            CurrentTurn =  _currentTurn,
            CurrentHealth = Enemy.Instance.CurrentHealth,
            MaxHealth = Enemy.Instance.MaxHealth,
            SlotLogic = slotLogic
        };
    }
}