using System;
using _02.Scripts.CoreSystem;
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
        [SerializeField] private TextMeshProUGUI infoTitleText;
        [SerializeField] private TextMeshProUGUI infoDescriptionText;
        [SerializeField] private TextMeshProUGUI actionSpeedText;
        
        private void Awake()
        {
            turnChannel.AddListener<TurnChangeEvent>(HandleTurnChange);
            gameChannel.AddListener<StageStartEvent>(HandleStageChange);
            gameChannel.AddListener<InfoShowEvent>(HandleInfoShow);
            gameChannel.AddListener<InfoHideEvent>(HandleInfoHide);
        }

        private void Start()
        {
            PresentationControl.OnSpeedChanged += UpdateLabel;
            UpdateLabel(PresentationControl.Speed); 
        }

        private void OnDestroy()
        {
            turnChannel.RemoveListener<TurnChangeEvent>(HandleTurnChange);
            gameChannel.RemoveListener<StageStartEvent>(HandleStageChange);
            gameChannel.RemoveListener<InfoShowEvent>(HandleInfoShow);
            gameChannel.RemoveListener<InfoHideEvent>(HandleInfoHide);
            PresentationControl.OnSpeedChanged -= UpdateLabel;
        }
        
        private void UpdateLabel(float speed) => actionSpeedText.text = $"x{speed:0.#}";

        private void HandleStageChange(StageStartEvent evt)
        {
            //stageText.text = (evt.StageIndex)+1 + "s";
        }

        private void HandleTurnChange(TurnChangeEvent evt)
        {
            turnText.text = evt.CurrentTurn.ToString();
        }
        
        private void HandleInfoShow(InfoShowEvent evt)
        {
            infoTitleText.text = evt.Title;
            infoDescriptionText.text = evt.Description;
        }
        
        private void HandleInfoHide(InfoHideEvent evt)
        {
            infoTitleText.text = string.Empty;
            infoDescriptionText.text = string.Empty;
        }
    }
}