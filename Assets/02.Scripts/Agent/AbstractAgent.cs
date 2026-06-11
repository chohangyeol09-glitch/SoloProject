using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.Agent
{
    public abstract class AbstractAgent : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }

        public virtual void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            OnHealthChanged();
            if (CurrentHealth <= 0)
                OnDead();

        }

        public virtual void Heal(int heal)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);
            OnHealthChanged();
        }

        protected abstract void OnHealthChanged();

        protected abstract void OnDead();
    }
}