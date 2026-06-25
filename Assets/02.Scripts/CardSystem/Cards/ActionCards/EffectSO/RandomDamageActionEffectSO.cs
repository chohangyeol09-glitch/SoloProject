using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    // Before 타이밍 권장: 공격마다 0 ~ maxDamage(등급별) 사이 랜덤값을 뽑아 공격력에 더한다.
    [CreateAssetMenu(fileName = "RandomDamageEffect", menuName = "Card/Action/Effect/RandomDamage")]
    public class RandomDamageActionEffectSO : AbstractActionEffectSO
    {
        [SerializeField] private GradeValue maxDamage;

        // 카드/설명에는 최대값을 표시한다.
        public override int GetDisplayValue(CardGrade grade) => maxDamage.Get(grade);

        public override bool IsActivate(EffectExecuteContext context) => maxDamage.Get(context.Grade) > 0;

        public override UniTask Apply(EffectExecuteContext context, List<AbstractSlot> targets)
        {
            int max = maxDamage.Get(context.Grade);
            int bonus = Random.Range(0, max + 1); // 0 ~ max 포함
            context.ActionCard.AddAttackValue(bonus);
            return UniTask.CompletedTask;
        }
    }
}
