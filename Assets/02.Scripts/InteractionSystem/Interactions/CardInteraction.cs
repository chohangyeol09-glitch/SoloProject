using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.InteractionSystem.Interactions
{
    public class CardInteraction : MonoBehaviour, IModule, IDraggable, IHoverable
    {
        public Transform Transform => _owner.transform;
        public LayerMask DropLayer { get; set; }

        private ModuleOwner _owner;
        private bool _isDragging = false;

        public event Action<Vector3> OnDragStart;
        public event Action<Vector3> OnDragUpdate;
        public event Action OnDragEnd;
        public event Action OnHoverEnter;
        public event Action OnHoverExit;

        public void Initialize(ModuleOwner owner) => _owner = owner;

        public void HandleDragStart(Vector3 mouseWorldPos)
        {
            _isDragging = true;
            OnDragStart?.Invoke(mouseWorldPos);
        }

        public void HandleDragging(Vector3 pos) => OnDragUpdate?.Invoke(pos);

        public void HandleDragEnd()
        {
            _isDragging = false;
            OnDragEnd?.Invoke();
        }

        public void HandleHoverEnter() => OnHoverEnter?.Invoke();

        public void HandleHoverExit()
        {
            if (_isDragging) return;
            OnHoverExit?.Invoke();
        }
    }
}