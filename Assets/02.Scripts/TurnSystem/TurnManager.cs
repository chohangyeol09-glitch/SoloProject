using Cysharp.Threading.Tasks;
using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.Enemys;
using _02.Scripts.Enemys.Boss;
using UnityEngine;

namespace _02.Scripts.TurnSystem
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EnemyPatternManager enemyPatternManager;

        public int TurnCount { get; private set; }

        private void Awake()
        {
            turnEventChannel.AddListener<TurnEndEvent>(HandleTurnEnd);
            gameEventChannel.AddListener<StageStartEvent>(HandleStageChange);
        }

        private void HandleStageChange(StageStartEvent evt)
        {
            TurnCount = 0; 
            TurnStart();
        }

        private void OnDestroy()
        {
            turnEventChannel.RemoveListener<TurnEndEvent>(HandleTurnEnd);
        }

        public void TurnStart()
        {
            TurnCount++;
            turnEventChannel.RaiseEvent(new TurnChangeEvent().Init(TurnCount));
        }

        private void HandleTurnEnd(TurnEndEvent evt) => TurnEndSequence().Forget();

        private async UniTaskVoid TurnEndSequence()
        {
            using (PresentationControl.Busy())
            {
                await RaiseClearHand();
                await RaiseExecuteCards();

                if (Enemy.Instance.IsDead)
                {
                    gameEventChannel.RaiseEvent(new StageClearEvent().Init(Enemy.Instance.Rewards));
                    return;
                }
                
                await RaiseGimmicks();
            }

            TurnStart();
        }

        private UniTask RaiseClearHand()
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            turnEventChannel.RaiseEvent(new ClearHandEvent().Init(() => tcs.TrySetResult()));
            return tcs.Task;
        }

        private UniTask RaiseExecuteCards()
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            turnEventChannel.RaiseEvent(new ExecuteCardsEvent().Init(() => tcs.TrySetResult()));
            return tcs.Task;
        }

        private UniTask RaiseGimmicks()
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            enemyPatternManager.ExecuteGimmicks(EnemyPatternTiming.OnTurnStart, TurnCount, () => tcs.TrySetResult());
            return tcs.Task;
        }
        
        #if UNITY_EDITOR
        
        [ContextMenu("TestTurnStart")]
        private void TestTurnStart() => TurnStart();
        
        #endif
    }
}