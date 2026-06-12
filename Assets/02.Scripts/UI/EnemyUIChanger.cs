using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace _02.Scripts.UI
{
    public class EnemyUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameChannel;
        [SerializeField] private EventChannelSO enemyChannel;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image isBossImg;
        
    
        private void Awake()
        {
            enemyChannel.AddListener<HealthChangedEvent>(HandleHealthChange);
            gameChannel.AddListener<StageStartEvent>(HandleEnemyInfoChange);
        }

        private void HandleEnemyInfoChange(StageStartEvent evt)
        {
            nameText.text = evt.EnemyData.EnemyName;
            if (evt.EnemyData.IsBoss)
                isBossImg.enabled = true;
            else
                isBossImg.enabled = false;
        }

        private void HandleHealthChange(HealthChangedEvent evt)
        {
            healthText.text = evt.CurrentHealth.ToString();
        }
    }
}
