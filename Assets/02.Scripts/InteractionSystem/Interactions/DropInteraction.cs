using System;
using System.Collections.Generic;
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
        private readonly HashSet<Type> _canDropTypes = new();

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        public void HandleDrop(Transform dropTrm)
        {
            if (_canDropTypes.Count > 0)
            {
                bool found = false;
                foreach (Type t in _canDropTypes)
                {
                    if (dropTrm.TryGetComponent(t, out Component _))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return;
            }
            OnDrop?.Invoke(dropTrm);
        }

        public void HandleHoverEnter() => OnHoverEnter?.Invoke();

        public void HandleHoverExit() => OnHoverExit?.Invoke();

        public void SetCanDropType(Type type)
        {
            _canDropTypes.Clear();
            _canDropTypes.Add(type);
        }

        public void AddCanDropType(Type type) => _canDropTypes.Add(type);
    }
}