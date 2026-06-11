using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class PlayerUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO playerEventChannel;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI healthText;
        

        private void Awake()
        {
            playerEventChannel.AddListener<CostChangedEvent>(HandleChangeCost);
            playerEventChannel.AddListener<HealthChangedEvent>(HandleChangeHealth);
        }

        private void OnDestroy()
        {
            playerEventChannel.RemoveListener<CostChangedEvent>(HandleChangeCost);
            playerEventChannel.RemoveListener<HealthChangedEvent>(HandleChangeHealth);
        }

        private void HandleChangeCost(CostChangedEvent evt)
        {
            costText.text = evt.CurrentCost.ToString();
        }

        private void HandleChangeHealth(HealthChangedEvent evt)
        {
            healthText.text = evt.CurrentHealth.ToString();
        }
        
    }
}