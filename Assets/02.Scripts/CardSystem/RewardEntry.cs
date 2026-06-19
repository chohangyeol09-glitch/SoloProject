using System;

namespace _02.Scripts.CardSystem
{
    public enum CardPoolType
    {
        Action,
        ActionEffect,
        Stat
    }

    // 적이 주는 보상 한 건: (카드 종류 + 등급). 처치 시 이 종류·등급 카드 3장 중 1장을 고른다.
    [Serializable]
    public class RewardEntry
    {
        public CardPoolType Type;
        public CardGrade Grade;
    }
}
