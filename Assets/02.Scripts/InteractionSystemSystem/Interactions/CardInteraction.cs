using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.InteractionSystemSystem.Interactions
{
    public class CardInteraction : MonoBehaviour, IModule, IDraggable, IHoverable
    {
        public Transform Transform => _owner.transform;
        private ModuleOwner _owner;

        public event Action OnDragStarted;
        public event Action<Vector3> OnDragUpdated;
        public event Action OnDragEnded;
        public event Action OnHoverEntered;
        public event Action OnHoverExited;
        
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void OnDragStart()
        {
            OnDragStarted?.Invoke();
        }

        public void OnDragging(Vector3 pos)
        {
            _owner.transform.position = pos;
            OnDragUpdated?.Invoke(pos);
        }

        public void OnDragEnd()
        {
            OnDragEnded?.Invoke();
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