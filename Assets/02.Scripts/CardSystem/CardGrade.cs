using System;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    public enum CardGrade
    {
        BRONZE,
        SILVER,
        GOLD
    }
    
    [Serializable]
    public class GradeSprite
    {
        public Sprite bronze;
        public Sprite silver;
        public Sprite gold;
    }
}