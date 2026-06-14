using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class GameUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO turnChannel;
        [SerializeField] private EventChannelSO gameChannel;

        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI stageText;

        private void Awake()
        {
            turnChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
            gameChannel.AddListener<StageStartEvent>(HandleStageChange);
        }

        private void OnDestroy()
        {
            turnChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
            gameChannel.RemoveListener<StageStartEvent>(HandleStageChange);
        }

        private void HandleStageChange(StageStartEvent evt)
        {
            //stageText.text = (evt.StageIndex)+1 + "s";
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            turnText.text = evt.CurrentTurn.ToString();
        }
    }
}