using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02.Scripts
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Control.IPlayerActions
    {
        public event Action<Vector2> OnMovePointer;
        public event Action OnClickDown;
        public event Action OnClickUp;
        public event Action<Vector2> OnPointerDelta;
        
        private Control _control;
        private Vector2 _lastMousePos;

        private void OnEnable()
        {
            if (_control == null)
            {
                _control = new Control();
                _control.Player.SetCallbacks(this);
            }
            _control.Player.Enable();
            
            _lastMousePos = Vector2.zero;
        }

        private void OnDisable()
        {
            _control.Player.Disable();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            Vector2 current = context.ReadValue<Vector2>();
            Vector2 delta = current - _lastMousePos;
            OnPointerDelta?.Invoke(delta);
            _lastMousePos = current;
            OnMovePointer?.Invoke(current);
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