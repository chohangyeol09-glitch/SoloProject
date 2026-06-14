using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractBossConditionSO : ScriptableObject
    {
        [field: SerializeField] public bool TriggerOnce { get; private set; }

        public abstract bool IsActivate(BossGimmickContext context);
    }
}