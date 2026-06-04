using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CardSystem.Cards.CardEnumSprite;
using _02.Scripts.CoreSystem.ModuleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCardUISetter : MonoBehaviour, IModule, IAfterInitializeModule
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

    [Header("SO")] [SerializeField] private SlotTargetRangeSpriteSO slotTargetRangeSpriteSO;
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
        NameText.text = _actionCard.ActionCardData.Name;
        mainIcon.sprite = _actionCard.ActionCardData.Icon;
        attackSlotTypeIcon.sprite = slotTargetRangeSpriteSO.GetSprite(_actionCard.ActionCardData.TargetRangeType);
        actionTypeIcon.sprite = actionTypeSpriteSO.GetSprite(_actionCard.ActionCardData.ActionCardType);
        
        foreach (AbstractActionEffectSO effectSO in _actionCard.ActionCardData.BeforeEffects)
        {
            GameObject img = Instantiate(effectIconPrefab, effectLayout.transform);
            img.GetComponent<Image>().sprite = effectSO.Icon;
        }

        foreach (AbstractActionEffectSO effectSO in _actionCard.ActionCardData.AfterEffects)
        {
            GameObject img = Instantiate(effectIconPrefab, effectLayout.transform);
            img.GetComponent<Image>().sprite = effectSO.Icon;
        }
        
        
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
