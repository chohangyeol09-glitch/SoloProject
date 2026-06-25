using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class InfoShowUI : MonoBehaviour
    {
        [SerializeField] private EventChannelSO eventChannel;

        [SerializeField] private TextMeshPro titleText;
        [SerializeField] private TextMeshPro descriptionText;
        private void Awake()
        {
            eventChannel.AddListener<InfoShowEvent>(HandleInfoShow);
            eventChannel.AddListener<InfoHideEvent>(HandleInfoHide);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<InfoShowEvent>(HandleInfoShow);
            eventChannel.RemoveListener<InfoHideEvent>(HandleInfoHide);
        }

        private void HandleInfoShow(InfoShowEvent obj)
        {
            titleText.text = obj.Title;
            descriptionText.text = obj.Description;
        }

        private void HandleInfoHide(InfoHideEvent obj)
        {
            titleText.text = "";
            descriptionText.text = "";
        }
    }
}