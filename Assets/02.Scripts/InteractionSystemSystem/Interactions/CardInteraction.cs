using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace _02.Scripts.InteractionSystemSystem.Interactions
{
    public class CardInteraction : MonoBehaviour, IModule, IDraggable, IHoverable
    {
        public Transform Transform => _owner.transform;
        public LayerMask DropLayer { get; set; }
        private ModuleOwner _owner;
        private bool _isDragging = false;

        public event Action OnDragStarted;
        public event Action<Vector3> OnDragUpdated;
        public event Action OnDragEnded;
        public event Action OnHoverEntered;
        public event Action OnHoverExited;

        [SerializeField] private float hoverOffset = 0;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void OnDragStart()
        {
            _isDragging = true;
            OnDragStarted?.Invoke();
        }

        public void OnDragging(Vector3 pos)
        {
            OnDragUpdated?.Invoke(pos);
        }

        public void OnDragEnd()
        {
            _isDragging = false;
            OnDragEnded?.Invoke();
        }

        public void OnHoverEnter()
        {
            OnHoverEntered?.Invoke();
        }

        public void OnHoverExit()
        {
            if (_isDragging) return;
            OnHoverExited?.Invoke();
        }
    }
}