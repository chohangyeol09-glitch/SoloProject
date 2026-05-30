using System;
using _02.Scripts.DragSystem;
using UnityEngine;

namespace _02.Scripts
{
    public class ObjectDragging : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInputSO; 
        
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private LayerMask slotLayer;
        [SerializeField] private LayerMask tableLayer;
        
        private Transform _hoveredObject;
        private IHoverable _hoveredSlotHoverable;
        private IDropTarget _hoveredSlot;
        private IDraggable _draggable;
        
        private bool _isDragging => _draggable != null;
        private Vector3 _tableRayPos;
        private Vector2 _mousePos;
        [SerializeField] private float yOffset = 2f;
        private Ray ray;
        
        
        private void Awake()
        {
            playerInputSO.OnMovePointer += MoveMousePosition;
            playerInputSO.OnClickDown += ClickDown;
            playerInputSO.OnClickUp += ClickUp;
        }

        private void OnDestroy()
        {
            playerInputSO.OnMovePointer -= MoveMousePosition;
            playerInputSO.OnClickDown -= ClickDown;
            playerInputSO.OnClickUp -= ClickUp;
        }

        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(_mousePos);
            
            if (Physics.Raycast(ray, out RaycastHit tableHit, Mathf.Infinity, tableLayer)) //ray가 맞은 위치 
            {
                _tableRayPos = tableHit.point;
                _tableRayPos.y += yOffset;
            }

            if (!_isDragging) //드래그 중이 아닐 때 
            {
                if (Physics.Raycast(ray, out RaycastHit cardHit, Mathf.Infinity, cardLayer)) //카드 감지
                {
                    if (_hoveredObject != cardHit.transform) //다른 카드로 옮기면 호버해제
                    {
                        _hoveredObject?.GetComponent<IHoverable>()?.OnHoverExit();
                        _hoveredObject = cardHit.transform;
                        _hoveredObject?.GetComponent<IHoverable>()?.OnHoverEnter();
                    }
                }
                else 
                {
                    _hoveredObject?.GetComponent<IHoverable>()?.OnHoverExit();
                    _hoveredObject = null;
                }
            }
            else
            {
                _draggable.OnDragging(_tableRayPos);

                if (Physics.Raycast(ray, out RaycastHit slotHit, Mathf.Infinity, slotLayer))
                {
                    IDropTarget dropTarget = slotHit.transform.GetComponent<IDropTarget>();
                    IHoverable hoverable = slotHit.transform.GetComponent<IHoverable>();

                    if (_hoveredSlot != dropTarget)
                    {
                        _hoveredSlotHoverable?.OnHoverExit(); 
                        _hoveredSlot = dropTarget;
                        _hoveredSlotHoverable = hoverable;
                        _hoveredSlotHoverable?.OnHoverEnter(); 
                    }
                }
                else
                {
                    _hoveredSlotHoverable?.OnHoverExit(); 
                    _hoveredSlot = null;
                    _hoveredSlotHoverable = null;
                }
            }
        }

        private void ClickDown()
        {
            if (_isDragging) return;
         
            Ray clickRay = Camera.main.ScreenPointToRay(_mousePos);
            
            if (Physics.Raycast(clickRay, out RaycastHit cardHit, Mathf.Infinity, cardLayer))
            {
                _draggable = cardHit.transform.GetComponent<IDraggable>();
                _draggable?.OnDragStart();

                _hoveredObject?.GetComponent<IHoverable>()?.OnHoverExit();
                _hoveredObject = null;
            }
        }

        private void ClickUp()
        {
            if (!_isDragging) return;

            if (_hoveredSlot != null)
            {
                _hoveredSlot?.OnDrop(_draggable.Transform);
                _hoveredSlotHoverable.OnHoverExit();
                _hoveredSlot = null;
                _hoveredSlotHoverable = null;
            }
            else
            {
                _draggable.OnDragEnd();
            }
            _draggable = null;
        }
        
        private void MoveMousePosition(Vector2 pos) => _mousePos = pos;
        
    }
}