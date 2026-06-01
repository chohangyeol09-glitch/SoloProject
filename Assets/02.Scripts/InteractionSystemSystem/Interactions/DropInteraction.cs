using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.InteractionSystemSystem.Interactions
{
    public class DropInteraction : MonoBehaviour, IModule, IDropTarget, IHoverable
    {
        public event Action<Transform> OnDropped;
        public event Action OnHoverEntered;
        public event Action OnHoverExited;
        
        private ModuleOwner _owner;
        private Type _isCanDropType;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void HandleDrop(Transform dropTrm)
        {
            if (_isCanDropType != null && !dropTrm.TryGetComponent(_isCanDropType, out _)) return;
            OnDropped?.Invoke(dropTrm);
        }

        public void OnHoverEnter()
        {
            OnHoverEntered?.Invoke();
        }

        public void OnHoverExit()
        {
            OnHoverExited?.Invoke();
        }
        
        public void SetCanDropType(Type type)
        {
            _isCanDropType = type;
        }
    }
}