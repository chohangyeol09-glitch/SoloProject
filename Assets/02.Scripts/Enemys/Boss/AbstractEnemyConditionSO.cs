using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractEnemyConditionSO : ScriptableObject
    {
        [field: SerializeField] public bool TriggerOnce { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        public abstract bool IsActivate(EnemyPatternContext context);
        
        public virtual string GetDescription() => Description;  
    }
}   