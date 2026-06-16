using System.Collections.Generic;
using _02.Scripts.Players;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "BerserkerEffect",menuName = "Card/Action/Effect/BerserkerEffect")]
    public class BerserkerActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private int targetHealth;
        
        public override bool IsActivate(EffectExecuteContext context)
            => Player.Instance.CurrentHealth <= Player.Instance.CurrentHealth * 0.3f;
        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.AddAttackValue(5);
            return UniTask.CompletedTask;
        }
    }
}