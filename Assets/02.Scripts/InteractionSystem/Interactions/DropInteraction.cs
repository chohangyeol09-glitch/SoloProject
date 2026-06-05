using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.InteractionSystem.Interactions
{
    public class DropInteraction : MonoBehaviour, IModule, IDropTarget, IHoverable
    {
        public event Action<Transform> OnDrop;
        public event Action OnHoverEnter;
        public event Action OnHoverExit;
        
        private ModuleOwner _owner;
        private Type _isCanDropType;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void HandleDrop(Transform dropTrm)
        {
            if (_isCanDropType != null && !dropTrm.TryGetComponent(_isCanDropType, out _)) return;
            OnDrop?.Invoke(dropTrm);
        }

        public void HandleHoverEnter()
        {
            OnHoverEnter?.Invoke();
        }

        public void HandleHoverExit()
        {
            OnHoverExit?.Invoke();
        }
        
        public void SetCanDropType(Type type)
        {
            _isCanDropType = type;
        }
    }
}