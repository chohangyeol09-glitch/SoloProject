using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.CardEnumSprite
{
    public abstract class AbstractEnumSpriteSO : ScriptableObject
    {
        public abstract Sprite GetSprite(int enumValue);
    }
}