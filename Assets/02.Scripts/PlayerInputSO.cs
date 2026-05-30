using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02.Scripts
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO", order = 0)]
    public class PlayerInputSO : ScriptableObject, Control.IPlayerActions
    {
        public event Action<Vector2> OnMovePointer;
        public event Action OnClickDown;
        public event Action OnClickUp;
        
        private Control _control;
        
        private void OnEnable()
        {
            if (_control == null)
            {
                _control = new Control();
                _control.Player.SetCallbacks(this);
            }
            _control.Player.Enable();
        }

        private void OnDisable()
        {
            _control.Player.Disable();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            OnMovePointer?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed) 
                OnClickDown?.Invoke();
            
            if (context.canceled) 
                OnClickUp?.Invoke();
        }
    }
}