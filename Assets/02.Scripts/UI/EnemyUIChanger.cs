using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using _02.Scripts.Enemys;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.UI
{
    public class EnemyUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameChannel;
        [SerializeField] private EventChannelSO enemyChannel;
        
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI nameText;

        [SerializeField] private Transform patternLayout;
        [SerializeField] private GameObject iconPrefab;

        private int _maxHealth;
        private void Awake()
        {
            enemyChannel.AddListener<HealthChangedEvent>(HandleHealthChange);
            gameChannel.AddListener<StageStartEvent>(HandleEnemyInfoChange);
        }

        private void HandleEnemyInfoChange(StageStartEvent evt)
        {
            _maxHealth = evt.EnemyData.MaxHealth;
            foreach (Transform child in patternLayout.transform)
            {
                Destroy(child.gameObject);
            }
            
            nameText.text = evt.EnemyData.EnemyName;
            if (evt.EnemyData.IsBoss)
                nameText.color = Color.softRed;
            else
                nameText.color = Color.white;

            foreach (Gimmick pattern in evt.EnemyData.Gimmicks)
            {
                GameObject obj = Instantiate(iconPrefab, patternLayout);
                obj.GetComponent<Image>().sprite = pattern.Pattern.Icon;
                InfoShowProp infoShow = obj.GetComponent<InfoShowProp>();
                infoShow.SetInfo(pattern.Pattern.Title, pattern.Condition.GetDescription() + ",\n" + pattern.Pattern.GetDescription());
            }
        }

        private void HandleHealthChange(HealthChangedEvent evt)
        {
            healthText.text = evt.CurrentHealth + " / " + _maxHealth;
        }
    }
}
