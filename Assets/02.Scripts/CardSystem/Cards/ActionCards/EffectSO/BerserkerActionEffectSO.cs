using System.Collections.Generic;
using _02.Scripts.Player;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "BerserkerAction",menuName = "Card/Action/Effect/BerserkerAction")]
    public class BerserkerActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private int targetHealth;
        
        public override bool IsActivate(EffectExecuteContext context) => PlayerDataManager.Instance.MaxHealth <= targetHealth;
        public override void Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.ChangeValue(5);
        }
    }
}