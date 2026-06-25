using System.Collections.Generic;
using _02.Scripts.CardSystem;
using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.Enemys;
using _02.Scripts.SlotSystem;
using _02.Scripts.Players;
using _02.Scripts.TurnSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageListSO stageList;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private StageTransitionAnimator transitionAnimator;

        [Header("Stage Clear Sequence")]
        [SerializeField] private EnemyAnimController enemyAnim;
        [SerializeField] private CardSelectionManager cardSelection;
        [SerializeField] private StageChanger stageChanger;

        [Header("Game Clear")]
        [SerializeField] private GameObject clearCanvas;

        private SlotLogic _slotLogic;
        private TurnManager _turnManager;
        private int _currentStageIndex = -1;

        private void Awake()
        {
            _slotLogic = FindObjectOfType<SlotLogic>();
            _turnManager = FindObjectOfType<TurnManager>();
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
            if (cardSelection != null)
                cardSelection.OnGameStartSelectionComplete += stageChanger.Show;
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
            if (cardSelection != null)
                cardSelection.OnGameStartSelectionComplete -= stageChanger.Show;
        }

        private void HandleStageClear(StageClearEvent evt) => StageClearSequence(evt.Rewards).Forget();

        private async UniTaskVoid StageClearSequence(List<RewardEntry> rewards)
        {
            using (PresentationControl.Busy())
            {
                await enemyAnim.PlayDeath();
                if (_slotLogic != null)
                    await cardSelection.CollectCardsFromPlayerSlots(_slotLogic.PlayerSlots);
                await transitionAnimator.PlayStageEndAsync();
                await cardSelection.ShowRewards(rewards);
            }

            if (stageChanger != null) stageChanger.Show();
        }

        public void StartNextStage()
        {
            if (stageChanger != null) stageChanger.gameObject.SetActive(false);

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
                if (clearCanvas != null) clearCanvas.SetActive(true);
                return;
            }

            EnemyDataSO stageData = stageList.Stages[_currentStageIndex];

            gameEventChannel.RaiseEvent(new StageStartEvent().Init(_currentStageIndex, stageData));
            _turnManager.TurnStart();
            Player.Instance.Heal(Player.Instance.MaxHealth / 10);
        }

        public void ClearStage() { }

#if UNITY_EDITOR
        [ContextMenu("StartNextStage")]
        public void TestStartNextStage() => StartNextStage();
#endif
    }
}
