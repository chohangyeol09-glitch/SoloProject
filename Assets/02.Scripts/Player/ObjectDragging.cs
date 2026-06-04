using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.InteractionSystemSystem;
using _02.Scripts.InteractionSystemSystem.Interactions;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class ObjectDragging : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInputSO;
        [SerializeField] private LayerMask cardLayer;
        [SerializeField] private LayerMask tableLayer;
        [SerializeField] private LayerMask draggingLayer;
        [SerializeField] private float yOffset = 2f;
        [SerializeField] private float dragSensitivity = 0.01f;
        
        private IHoverable _hoveredObject;
        private IHoverable _hoveredSlotHoverable;
        private IDropTarget _hoveredSlot;
        private IDraggable _draggable;
        private int _dragObjOriginLayer;

        private Vector3 _tableRayPos;
        private Vector2 _mousePos;
        private Transform _lastDropHit;
        private Ray _ray;

        private bool _isDragging => _draggable != null;

        private void Awake()
        {
            playerInputSO.OnMovePointer += MoveMousePosition;
            playerInputSO.OnPointerDelta += MoveDraggable;
            playerInputSO.OnClickDown += ClickDown;
            playerInputSO.OnClickUp += ClickUp;
        }

        private void OnDestroy()
        {
            playerInputSO.OnMovePointer -= MoveMousePosition;
            playerInputSO.OnPointerDelta -= MoveDraggable;
            playerInputSO.OnClickDown -= ClickDown;
            playerInputSO.OnClickUp -= ClickUp;
        }

        private void Update()
        {
            _ray = Camera.main.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(_ray, out RaycastHit tableHit, Mathf.Infinity, tableLayer))
            {
                _tableRayPos = tableHit.point;
                _tableRayPos.y += yOffset;
            }

            if (!_isDragging)
            {
                if (Physics.Raycast(_ray, out RaycastHit cardHit, Mathf.Infinity, cardLayer))
                {
                    CardInteraction cardInteraction = cardHit.transform.GetComponent<AbstractCard>().CardInteraction;
                    if (cardInteraction == null) return;

                    if (_hoveredObject != cardInteraction)
                    {
                        _hoveredObject?.OnHoverExit();
                        _hoveredObject = cardInteraction;
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
                //_draggable.OnDragging(_tableRayPos);

                if (Physics.Raycast(_ray, out RaycastHit dropHit, Mathf.Infinity, _draggable.DropLayer))
                {
                    if (_lastDropHit == dropHit.transform) return;
                    
                    _hoveredSlotHoverable?.OnHoverExit();
                    _lastDropHit = dropHit.transform;

                    DropInteraction interaction = null;

                    //임시
                    if (dropHit.transform.TryGetComponent<PlayerSlot>(out var slot))
                        interaction = slot.DropInteraction;
                    else if (dropHit.transform.TryGetComponent<ActionCard>(out var card))
                        interaction = card.DropInteraction;

                    _hoveredSlot = interaction;
                    _hoveredSlotHoverable = interaction;
                    _hoveredSlotHoverable?.OnHoverEnter();
                }
                else
                {
                    _hoveredSlotHoverable?.OnHoverExit();
                    _hoveredSlot = null;
                    _hoveredSlotHoverable = null;
                    _lastDropHit = null;
                }
            }
        }

        private void ClickDown()
        {
            if (_isDragging) return;
            Ray clickRay = Camera.main.ScreenPointToRay(_mousePos);

            if (!Physics.Raycast(clickRay, out RaycastHit cardHit, Mathf.Infinity, cardLayer)) return;
            
            _draggable = cardHit.transform.GetComponent<AbstractCard>().CardInteraction;
            _draggable?.OnDragStart();
            _hoveredObject?.OnHoverExit();
            _hoveredObject = null;
                
            _dragObjOriginLayer = _draggable.Transform.gameObject.layer;
            _draggable.Transform.gameObject.layer = draggingLayer;
        }

        private void ClickUp()
        {
            if (!_isDragging) return;

            Ray clickRay = Camera.main.ScreenPointToRay(_mousePos);
            if (Physics.Raycast(clickRay, out RaycastHit dropHit, Mathf.Infinity, _draggable.DropLayer))
            {
                DropInteraction interaction = null;

                if (dropHit.transform.TryGetComponent<PlayerSlot>(out var slot))
                    interaction = slot.DropInteraction;
                else if (dropHit.transform.TryGetComponent<ActionCard>(out var card))
                {
                    interaction = card.DropInteraction;
                }
                interaction?.HandleDrop(_draggable.Transform);
                _hoveredSlotHoverable?.OnHoverExit();
                _hoveredSlot = null;
                _hoveredSlotHoverable = null;
                _lastDropHit = null;
            }
            else
            {
                _draggable.OnDragEnd();
            }

            _draggable.Transform.gameObject.layer = _dragObjOriginLayer;
            
            _draggable = null;
        }

        private void MoveMousePosition(Vector2 pos) => _mousePos = pos;
        private void MoveDraggable(Vector2 delta)
        {
            if (!_isDragging) return;

            Vector3 worldDelta = Camera.main.transform.right * delta.x * dragSensitivity
                                 + Camera.main.transform.up * delta.y * dragSensitivity;

            _draggable.OnDragging(_draggable.Transform.position + worldDelta);
        }
    }
}