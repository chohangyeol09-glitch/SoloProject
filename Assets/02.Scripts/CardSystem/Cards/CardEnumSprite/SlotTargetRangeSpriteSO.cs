using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.CardEnumSprite
{
    [CreateAssetMenu(fileName = "SlotTargetRangeSprite", menuName = "Card/EnumSpriteSO/SlotTargetRangeSprite", order = 0)]

    public class SlotTargetRangeSpriteSO : AbstractEnumSpriteSO
    {
        [Serializable]
        private class Entry
        {
            public SlotTargetRangeType type;
            public Sprite sprite;
        }
        
        [SerializeField] private List<Entry> entries = new();
        
        public override Sprite GetSprite(int enumValue)
            => entries.Find(x => (int)x.type == enumValue)?.sprite;
        
        public Sprite GetSprite(SlotTargetRangeType type)
            => entries.Find(x => x.type == type)?.sprite;
    }
}