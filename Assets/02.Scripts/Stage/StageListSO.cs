using System.Collections.Generic;
using _02.Scripts.Enemys;
using UnityEngine;

namespace _02.Scripts.Stage
{
    [CreateAssetMenu(fileName = "StageList", menuName = "SO/Stage/StageList", order = 0)]
    public class StageListSO : ScriptableObject
    {
        [field: SerializeField] public List<EnemyDataSO> Stages { get; private set; } = new();
    }
}