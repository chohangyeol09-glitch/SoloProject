using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CardSystem.Cards.CardEnumSprite;
using _02.Scripts.CoreSystem.ModuleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.UI
{
    public class ActionCardUIChanger : MonoBehaviour, IModule, IAfterInitializeModule
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private Image mainIcon;
        [SerializeField] private Image attackSlotTypeIcon;
        [SerializeField] private Image actionTypeIcon;
        [SerializeField] private Transform effectLayout;
        [SerializeField] private GameObject effectIconPrefab;
        [SerializeField] private TextMeshProUGUI attackValueText;
        [SerializeField] private TextMeshProUGUI defenseValueText;

        [Header("SO")] 
        [SerializeField] private SlotTargetRangeSpriteSO slotTargetRangeSpriteSO;
        [SerializeField] private ActionTypeSpriteSO actionTypeSpriteSO;
    
    
        private ModuleOwner _owner;
        private ActionCard _actionCard;
    
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _actionCard = _owner.GetComponent<ActionCard>();
            _actionCard.OnAttackValueChanged += HandleAttackValueChanged;
            _actionCard.OnDefenseValueChanged += HandleDefenseValueChanged;
        
        }

        public void AfterInitialize()
        {
            if (_actionCard.ActionCardData == null) return;
            SetUI(_actionCard.ActionCardData);
        }

        public void SetUI(ActionCardDataSO data)
        {
            NameText.text = data.Name;
            mainIcon.sprite = data.Icon;
            attackSlotTypeIcon.sprite = slotTargetRangeSpriteSO.GetSprite(data.TargetRangeType);
            actionTypeIcon.sprite = actionTypeSpriteSO.GetSprite(data.ActionCardType);

            foreach (AbstractActionEffectSO effectSO in data.BeforeEffects)
                SpawnEffectIcon(effectSO);
            foreach (AbstractActionEffectSO effectSO in data.AfterEffects)
                SpawnEffectIcon(effectSO);
        }

        public void AddEffectIcon(AbstractActionEffectSO effect)
        {
            SpawnEffectIcon(effect);
        }

        private void SpawnEffectIcon(AbstractActionEffectSO effect)
        {
            GameObject obj = Instantiate(effectIconPrefab, effectLayout.transform);
            obj.GetComponent<EffectIconView>().Setup(effect);
        }

        private void HandleAttackValueChanged(int value)
        {
            attackValueText.text = value.ToString();
        }

        private void HandleDefenseValueChanged(int value)
        {
            defenseValueText.text = value.ToString();
        }
    }
}
