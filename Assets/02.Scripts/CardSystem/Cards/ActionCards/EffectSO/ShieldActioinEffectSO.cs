using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "ShieldEffect",menuName = "Card/Action/Effect/ShieldEffect")]
    public class ShieldActioinEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private int shieldValue = 2;
        
        public override bool IsActivate(EffectExecuteContext context) => true;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            context.ActionCard.AddDefenseValue(2);
            return UniTask.CompletedTask;
        }
    }
}