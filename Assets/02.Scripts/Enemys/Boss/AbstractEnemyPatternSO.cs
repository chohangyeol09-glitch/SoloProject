using Cysharp.Threading.Tasks;
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
        
        public virtual UniTask Execute(EnemyPatternContext context)
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            bool patternDone = false;
            bool animDone = false;

            void TryComplete()
            {
                if (patternDone && animDone) tcs.TrySetResult();
            }

            async UniTaskVoid RunEffect()
            {
                await ExecutePattern(context);
                patternDone = true;
                TryComplete();
            }

            _enemyChannel.RaiseEvent(new EnemyPatternStartEvent().Init(
                PatternTrigger,
                () => RunEffect().Forget(),
                () => { animDone = true; TryComplete(); }
            ));

            return tcs.Task;
        }

        protected abstract UniTask ExecutePattern(EnemyPatternContext context);
        
        public virtual string GetDescription() => Description;        
    }
}