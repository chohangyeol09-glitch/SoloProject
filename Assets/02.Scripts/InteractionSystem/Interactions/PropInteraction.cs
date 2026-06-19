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

        public Outlinable Outline { get; private set; }
        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            Outline = _owner.transform.GetComponent<Outlinable>();
                
        }

        public void HandleClick()
        {
            OnClick?.Invoke();
        }

        public void HandleHoverEnter()
        {
            Outline.FrontParameters.Color = Color.white;
            OnHoverEnter?.Invoke();   
        }

        public void HandleHoverExit()
        {
            Outline.FrontParameters.Color = Color.clear;
            OnHoverExit?.Invoke();
        }
    }
}