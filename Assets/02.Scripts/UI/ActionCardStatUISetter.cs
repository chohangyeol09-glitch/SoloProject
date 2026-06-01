using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CoreSystem.ModuleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCardStatUISetter : MonoBehaviour, IModule, IAfterInitializeModule
{
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _defenseText;
    
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
    }

    private void HandleAttackValueChanged(int value)
    {
        _attackText.text = value.ToString();
    }

    private void HandleDefenseValueChanged(int value)
    {
        _defenseText.text = value.ToString();
    }
}
