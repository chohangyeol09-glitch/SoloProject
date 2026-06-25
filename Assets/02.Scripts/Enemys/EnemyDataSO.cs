using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.Enemys.Boss;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
    public class EnemyDataSO : ScriptableObject
    {
        [field: SerializeField] public string EnemyName { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; } = 10;
        [field: SerializeField] public bool IsBoss { get; private set; }
        [field: SerializeField] public GameObject ModelPrefab { get; private set; }
        [field: SerializeField] public List<EnemyCardPlacement> CardPlacements { get; private set; } = new();

        [field: SerializeField] public List<RewardEntry> Rewards { get; private set; } = new();

        [field: SerializeField] public List<Gimmick> Gimmicks { get; private set; } = new();

        [field: SerializeField] public int RespawnTurnDelay { get; private set; } = 2;
        [field: SerializeField] public int MaxRespawnCount { get; private set; } = 0;
        [field: SerializeField] public List<EnemyRespawnEntry> RespawnPool { get; private set; } = new();
    }

    [Serializable]
    public class EnemyCardPlacement
    {
        public int SlotIndex;
        public ActionCardDataSO CardData;
        [field: SerializeField] public int AttackValue { get; private set; }
        [field: SerializeField] public int DefenseValue { get; private set; }
    }

    [Serializable]
    public class EnemyRespawnEntry
    {
        public ActionCardDataSO CardData;
        [field: SerializeField] public int AttackValue { get; private set; }
        [field: SerializeField] public int DefenseValue { get; private set; }
    }

    [Serializable]
    public class Gimmick
    {
        public EnemyPatternTiming Timing;
        public AbstractEnemyConditionSO Condition; 
        public AbstractEnemyPatternSO Pattern;     
    }
}