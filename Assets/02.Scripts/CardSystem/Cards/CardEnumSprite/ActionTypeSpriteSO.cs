using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionSO;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.CardEnumSprite
{
    [CreateAssetMenu(fileName = "ActionTypeSprite", menuName = "Card/EnumSpriteSO/ActionTypeSprite", order = 0)]
    public class ActionTypeSpriteSO : AbstractEnumSpriteSO
    {
        [Serializable]
        private class Entry
        {
            public ActionCardType type;
            public Sprite sprite;
        }
        
        [SerializeField] private List<Entry> entries;
        
        public override Sprite GetSprite(int enumValue) 
            => entries.Find(x => (int)x.type == enumValue)?.sprite;
        
        public Sprite GetSprite(ActionCardType type)
            => entries.Find(x => x.type == type)?.sprite;
    }
}