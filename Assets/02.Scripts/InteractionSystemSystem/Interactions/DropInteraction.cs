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
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void OnDrop(Transform dropTrm)
        {
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
    }
}