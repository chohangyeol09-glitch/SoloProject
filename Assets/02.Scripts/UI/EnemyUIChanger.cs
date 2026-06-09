using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class EnemyUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO enemyChannel;
        [SerializeField] private TextMeshProUGUI healthText;
    
        private void Awake()
        {
            enemyChannel.AddListener<HealthChangedEvent>(HandleHealthChange);
        }

        private void HandleHealthChange(HealthChangedEvent evt)
        {
            healthText.text = evt.CurrentHealth.ToString();
        }
    }
}
