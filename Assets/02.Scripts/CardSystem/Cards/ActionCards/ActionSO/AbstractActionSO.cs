using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    public abstract class AbstractActionSO : ScriptableObject
    {
        public abstract UniTask Execute(ActionCard card, List<AbstractSlot> targets);
    }
}