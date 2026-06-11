using System;
using System.Collections.Generic;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    public abstract class AbstractActionSO : ScriptableObject
    {
        public abstract void Execute(ActionCard card, List<AbstractSlot> targets, Action onComplete = null, Action<int> onPlayerDamage = null);
    }
}