using System;
using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts
{
    public class ObjectDragging : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInputSO; 
        
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private LayerMask slotLayer;
        [SerializeField] private LayerMask tableLayer;
        [SerializeField] private float yOffset = 2f;
        
        private IHoverable _hoveredObject;
        private IHoverable _hoveredSlotHoverable;
        private IDropTarget _hoveredSlot;
        private IDraggable _draggable;
        
        private Vector3 _tableRayPos;
        private Vector2 _mousePos;
        private Transform _lastSlotHit;
        private Ray _ray;
        
        private bool _isDragging => _draggable != null;
        
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
            _ray = Camera.main.ScreenPointToRay(_mousePos);
   
            if (Physics.Raycast(_ray, out RaycastHit tableHit, Mathf.Infinity, tableLayer)) //ray가 맞은 위치 
            {
                _tableRayPos = tableHit.point;
                _tableRayPos.y += yOffset;
            }

            if (!_isDragging) //드래그 중이 아닐 때 
            {
                if (Physics.Raycast(_ray, out RaycastHit cardHit, Mathf.Infinity, cardLayer)) //카드 감지
                {
                    if (!cardHit.transform.TryGetComponent<ActionCard>(out ActionCard card)) return;
                    if (_hoveredObject != card.CardInteraction ) //다른 카드로 옮기면 호버해제
                    {
                        _hoveredObject?.OnHoverExit();
                        _hoveredObject = card.CardInteraction;
                        _hoveredObject.OnHoverEnter();
                    }
                }
                else 
                {
                    _hoveredObject?.OnHoverExit();
                    _hoveredObject = null;
                }
            }
            else
            {
                _draggable.OnDragging(_tableRayPos);

                if (Physics.Raycast(_ray, out RaycastHit slotHit, Mathf.Infinity, slotLayer))
                {
                    if (_lastSlotHit != slotHit.transform) // 바뀔 때만 GetComponent
                    {
                        _hoveredSlotHoverable?.OnHoverExit();
                        _lastSlotHit = slotHit.transform;
                        if (!slotHit.transform.TryGetComponent<PlayerSlot>(out PlayerSlot playerSlot)) return;
                        _hoveredSlot = playerSlot.DropInteraction;
                        _hoveredSlotHoverable = playerSlot.DropInteraction;
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
            Debug.Log("click down");
         
            Ray clickRay = Camera.main.ScreenPointToRay(_mousePos);
            
            if (Physics.Raycast(clickRay, out RaycastHit cardHit, Mathf.Infinity, cardLayer))
            {
                _draggable = cardHit.transform.GetComponent<AbstractCard>().CardInteraction;
                _draggable?.OnDragStart();

                _hoveredObject?.OnHoverExit();
                _hoveredObject = null;
            }
        }

        private void ClickUp()
        {
            if (!_isDragging) return;

            Ray clickRay = Camera.main.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(clickRay, out RaycastHit slotHit, Mathf.Infinity, slotLayer))
            {
                slotHit.transform.TryGetComponent<PlayerSlot>(out PlayerSlot playerSlot);
                DropInteraction interaction = playerSlot.DropInteraction;
                interaction?.OnDrop(_draggable.Transform);
                _hoveredSlotHoverable?.OnHoverExit();
                _hoveredSlot = null;
                _hoveredSlotHoverable = null;
                _lastSlotHit = null;
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