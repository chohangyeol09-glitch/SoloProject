using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using UnityEngine;

namespace _02.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;
        [field: SerializeField] public bool IsBoss { get; private set; }

        [field: SerializeField] public List<EnemyCardPlacement> CardPlacements { get; private set; } = new();

        [field: SerializeField] public List<BossGimmick> BossGimmicks { get; private set; } = new();
    }

    [Serializable]
    public class EnemyCardPlacement
    {
        public int SlotIndex;
        public ActionCardDataSO CardData; // ← CardPrefab 제거, 데이터만
    }

    [Serializable]
    public class BossGimmick
    {
        public int TriggerTurn;   
        
    }
}