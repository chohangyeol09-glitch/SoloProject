using _02.Scripts.CoreSystem;
using UnityEngine;

namespace _02.Scripts
{
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        public int MaxCost;
        public int CurrentCost;
        public int Health;
        
        protected override void Awake()
        {
            base.Awake();
        }
    }
}