using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents.BossEvents;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractBossPatternSO : ScriptableObject
    {
        [field: SerializeField] public string PatternTrigger { get; private set; }
        [SerializeField] private EventChannelSO _enemyChannel;
        
        
        public void Execute(BossGimmickContext context, Action onComplete = null)
        {
            _enemyChannel.RaiseEvent(new BossPatternStartEvent().Init(
                PatternTrigger,
                () => ExecutePattern(context, null), 
                onComplete
            ));
        }

        protected abstract void ExecutePattern(BossGimmickContext context, Action onComplete);
    }
}