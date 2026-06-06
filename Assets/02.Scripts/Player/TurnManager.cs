using System.Collections;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO turnEventChannel;

        public int TurnCount { get; private set; }

        private void Awake()
        {
            turnEventChannel.AddListener<TurnEndEvent>(HandleTurnEnd);
        }

        private void OnDestroy()
        {
            turnEventChannel.RemoveListener<TurnEndEvent>(HandleTurnEnd);
        }

        public void TurnStart()
        {
            TurnCount++;
            turnEventChannel.RaiseEvent(new TurnStartEvent().Init(TurnCount));
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

            TurnStart();
        }
        
        #if UNITY_EDITOR
        
        [ContextMenu("TestTurnStart")]
        private void TestTurnStart() => TurnStart();
        
        #endif
    }
}