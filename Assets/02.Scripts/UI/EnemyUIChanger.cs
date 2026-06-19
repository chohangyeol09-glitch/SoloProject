using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
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
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Transform patternLayout;
        [SerializeField] private GameObject iconPrefab;

        private int _maxHealth;
        private void Awake()
        {
            enemyChannel.AddListener<HealthChangedEvent>(HandleHealthChange);
            enemyChannel.AddListener<InfoShowEvent>(HandleInfoShow);
            enemyChannel.AddListener<InfoHideEvent>(HandleInfoHide);
            gameChannel.AddListener<StageStartEvent>(HandleEnemyInfoChange);
        }

        private void HandleHealthChange(HealthChangedEvent evt)
        {
            healthText.text = evt.CurrentHealth + " / " + _maxHealth;
        }

        private void HandleEnemyInfoChange(StageStartEvent evt)
        {
            _maxHealth = evt.EnemyData.MaxHealth;
            healthText.text = evt.EnemyData.MaxHealth + " / " + _maxHealth;
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
                infoShow.SetInfo(pattern.Pattern.Title, pattern.Condition.GetDescription() + "\n" + pattern.Pattern.GetDescription());
            }
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
