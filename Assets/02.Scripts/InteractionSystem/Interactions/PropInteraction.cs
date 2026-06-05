using System;
using _02.Scripts.CoreSystem.ModuleSystem;
using EPOOutline;
using UnityEngine;

namespace _02.Scripts.InteractionSystem.Interactions
{
    public class PropInteraction :MonoBehaviour, IModule, IClickable, IHoverable
    {
        public event Action OnHoverEnter;
        public event Action OnHoverExit;
        public event Action OnClick;

        private Outlinable _outline;
        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _outline = _owner.transform.GetComponent<Outlinable>();
        }

        public void HandleClick()
        {
            OnClick?.Invoke();
        }

        public void HandleHoverEnter()
        {
            _outline.OutlineParameters.Color = Color.white;
            OnHoverEnter?.Invoke();   
        }

        public void HandleHoverExit()
        {
            _outline.OutlineParameters.Color = Color.clear;
            OnHoverExit?.Invoke();
        }
    }
}