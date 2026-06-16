using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents.BossEvents;
using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractEnemyPatternSO : ScriptableObject
    {
        [field: SerializeField] public string PatternTrigger { get; private set; }
        [SerializeField] protected EventChannelSO _enemyChannel;

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Title {get; private set;}
        [field: SerializeField] public string Description { get; private set; }
        
        public virtual void Execute(EnemyPatternContext context, Action onComplete = null)
        {
            bool patternDone = false;
            bool animDone = false;

            void TryComplete()
            {
                if (patternDone && animDone) onComplete?.Invoke();
            }
            
            Debug.Log(PatternTrigger);
            _enemyChannel.RaiseEvent(new EnemyPatternStartEvent().Init(
                PatternTrigger, 
            () => ExecutePattern(context, 
             () => { patternDone = true; TryComplete(); }),
             () => { animDone = true; TryComplete(); }
            ));
            Debug.Log($"patternDone: {patternDone}, animDone: {animDone}");
        }

        protected abstract void ExecutePattern(EnemyPatternContext context, Action onComplete);
        
        public virtual string GetDescription() => Description;        
    }
}