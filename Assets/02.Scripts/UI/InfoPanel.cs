using System;
using System.Diagnostics.Tracing;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameChannel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private float xOffset;

        private RectTransform _rect;
        private void Awake()
        {
            gameChannel.AddListener<InfoShowEvent>(HandleInfoShow);
            gameChannel.AddListener<InfoHideEvent>(HandleInfoHide);
            _rect = GetComponent<RectTransform>();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;   
        }

        private void HandleInfoShow(InfoShowEvent evt)
        {
            canvasGroup.alpha = 1;   
            Title.text = evt.Title;
            Description.text = evt.Description;
        }
        
        private void HandleInfoHide(InfoHideEvent evt)
        {
            canvasGroup.alpha = 0;
        }
    }
}