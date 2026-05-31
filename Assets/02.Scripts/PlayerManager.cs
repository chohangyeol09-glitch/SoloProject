using System;
using _02.Scripts.CoreSystem;
using UnityEngine;

namespace _02.Scripts
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        public event Action<int> OnChangeCost;
        public event Action<int> OnChangeHealth;
        public event Action OnDead;
        
        [SerializeField] private int _maxCost = 0;
        private int currentCost;
        [SerializeField] private int _currentHealth;

        
        protected override void Awake()
        {
            base.Awake();
            
            //test 
            currentCost = _maxCost;
            OnChangeCost?.Invoke(currentCost);
            OnChangeHealth?.Invoke(_currentHealth);
            
        }

        public bool ChangeCost(int value)
        {
            if (currentCost + value < 0)
                return false;

            currentCost += value;
            OnChangeCost?.Invoke(currentCost);
            return true;
        }

        public void ChangeHealth(int value)
        {
            if (_currentHealth + value <= 0)
            {
                _currentHealth = 0;
                OnDead?.Invoke();
            }
            
            _currentHealth += value;
            OnChangeHealth?.Invoke(_currentHealth);
        }
    }
}