using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class GameUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO turnChannel;
        [SerializeField] private EventChannelSO gameChannel;

        [SerializeField] private TextMeshPro turnText;
        [SerializeField] private TextMeshPro stageText;

        private void Awake()
        {
            turnChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
            gameChannel.AddListener<StageStartEvent>(HandleStageChange);
        }

        private void HandleStageChange(StageStartEvent evt)
        {
            stageText.text = (evt.StageIndex)+1 + " 스테이지";
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            turnText.text = evt.CurrentTurn + "번째 턴";
        }
    }
}