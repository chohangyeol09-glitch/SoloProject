using System;
using UnityEngine;

namespace _02.Scripts.CardSystem
{
    // 등급별 정수 값. 카드 등급에 맞는 값을 반환한다.
    [Serializable]
    public struct GradeValue
    {
        [SerializeField] private int bronze;
        [SerializeField] private int silver;
        [SerializeField] private int gold;

        public int Get(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronze,
            CardGrade.SILVER => silver,
            CardGrade.GOLD   => gold,
            _                => bronze
        };
    }

    // 등급별 실수 값.
    [Serializable]
    public struct GradeFloat
    {
        [SerializeField] private float bronze;
        [SerializeField] private float silver;
        [SerializeField] private float gold;

        public float Get(CardGrade grade) => grade switch
        {
            CardGrade.BRONZE => bronze,
            CardGrade.SILVER => silver,
            CardGrade.GOLD   => gold,
            _                => bronze
        };
    }
}
