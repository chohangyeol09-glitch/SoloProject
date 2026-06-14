using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.Enemys;
using _02.Scripts.Players;
using UnityEngine;

namespace _02.Scripts.Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageListSO stageList;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private EventChannelSO turnEventChannel;
        [SerializeField] private StageTransitionAnimator transitionAnimator;

        private int _currentStageIndex = -1;

        private void Awake()
        {
            enemyEventChannel.AddListener<EnemyDeadEvent>(HandleEnemyDead);
            enemyEventChannel.AddListener<EnemyDieEndEvent>(HandleEnemyDieEnd);
        }

        private void OnDestroy()
        {
            enemyEventChannel.RemoveListener<EnemyDeadEvent>(HandleEnemyDead);
            enemyEventChannel.RemoveListener<EnemyDieEndEvent>(HandleEnemyDieEnd);
        }

        public void StartNextStage()
        {
            if (transitionAnimator != null)
                transitionAnimator.PlayStageStartTransition(ExecuteStageStart);
            else
                ExecuteStageStart();
        }

        private void ExecuteStageStart()
        {
            _currentStageIndex++;

            if (_currentStageIndex >= stageList.Stages.Count)
            {
                Debug.Log("모든 스테이지 클리어!");
                return;
            }

            EnemyDataSO stageData = stageList.Stages[_currentStageIndex];

            gameEventChannel.RaiseEvent(new StageStartEvent().Init(_currentStageIndex, stageData));

            turnEventChannel.RaiseEvent(new TurnChangeEvent().Init(1));
            Player.Instance.Heal(Player.Instance.MaxHealth / 10);
        }

        public void ClearStage()
        {

        }

        private void HandleEnemyDead(EnemyDeadEvent evt)
        {
            gameEventChannel.RaiseEvent(new StageClearEvent());
        }

        private void HandleEnemyDieEnd(EnemyDieEndEvent evt)
        {
            ClearStage();
            transitionAnimator?.PlayStageEndTransition();
        }

#if UNITY_EDITOR
        [ContextMenu("StartNextStage")]
        public void TestStartNextStage() => StartNextStage();
#endif
    }
}