using System.Collections.Generic;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.EffectSO
{
    [CreateAssetMenu(fileName = "ActionEffectListSO", menuName = "Card/Action/Effect/EffectList", order = 0)]
    public class ActionEffectListSO : ScriptableObject
    {
        [field: SerializeField] public List<AbstractActionEffectSO> Effects;
    }
}