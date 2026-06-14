using System.Collections;
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
        [SerializeField] private BossGimmickManager bossGimmickManager;

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
            Debug.Log($"TurnStart| CurrentTurn: {TurnCount}");
            turnEventChannel.RaiseEvent(new TurnChangeEvent().Init(TurnCount));
        }

        private void HandleTurnEnd(TurnEndEvent evt)
        {
            StartCoroutine(TurnEndSequence());
        }

        private IEnumerator TurnEndSequence()
        {
            turnEventChannel.RaiseEvent(new InteractionDisableEvent());

            bool handCleared = false;
            turnEventChannel.RaiseEvent(new ClearHandEvent().Init(() => handCleared = true));
            yield return new WaitUntil(() => handCleared);

            bool cardsExecuted = false;
            turnEventChannel.RaiseEvent(new ExecuteCardsEvent().Init(() => cardsExecuted = true));
            yield return new WaitUntil(() => cardsExecuted);
            
            if (Enemy.Instance.IsDead)
            {
                gameEventChannel.RaiseEvent(new StageClearEvent());
                Debug.Log("Stage Clear");
                yield break;
            }

            bool gimmickDone = false;
            bossGimmickManager.ExecuteGimmicks(BossGimmickTiming.OnTurnStart, TurnCount, () => gimmickDone = true);
            yield return new WaitUntil(() => gimmickDone);

            TurnStart();
        }
        
        #if UNITY_EDITOR
        
        [ContextMenu("TestTurnStart")]
        private void TestTurnStart() => TurnStart();
        
        #endif
    }
}