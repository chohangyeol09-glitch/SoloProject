using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractBossConditionSO : ScriptableObject
    {
        public abstract bool IsActivate(BossGimmickContext context);   
    }
}