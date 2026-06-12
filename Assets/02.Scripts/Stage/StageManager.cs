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

        private int _currentStageIndex = -1;

        private void Awake()
        {
            enemyEventChannel.AddListener<EnemyDeadEvent>(HandleEnemyDead);
        }

        private void OnDestroy()
        {
            enemyEventChannel.RemoveListener<EnemyDeadEvent>(HandleEnemyDead);
        }

        public void StartNextStage()
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

        private void HandleEnemyDead(EnemyDeadEvent evt)
        {
            gameEventChannel.RaiseEvent(new StageClearEvent());
        }

#if UNITY_EDITOR
        [ContextMenu("StartNextStage")]
        public void TestStartNextStage() => StartNextStage();
#endif
    }
}