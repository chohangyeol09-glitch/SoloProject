using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.InteractionSystem;
using _02.Scripts.InteractionSystem.Interactions;
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
        [SerializeField] private LayerMask propLayer;
        [SerializeField] private float dragUpOffset = 1.5f;
        [SerializeField] private float dragSensitivity = 0.01f;

        private IHoverable _hoveredObject;
        private IHoverable _hoveredSlotHoverable;
        private IDropTarget _hoveredSlot;
        private IDraggable _draggable;
        private IHoverable _hoveredProp;
        private IClickable _clickedProp;
        private int _dragObjOriginLayer;

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

            if (!_isDragging)
            {
                if (Physics.Raycast(_ray, out RaycastHit cardHit, Mathf.Infinity, cardLayer))
                {
                    CardInteraction cardInteraction = null;
                    if (cardHit.transform.TryGetComponent<AbstractCard>(out var card))
                        cardInteraction = card.CardInteraction;
                    if (cardInteraction == null) return;

                    if (_hoveredObject != cardInteraction)
                    {
                        _hoveredObject?.HandleHoverExit();
                        _hoveredObject = cardInteraction;
                        _hoveredObject.HandleHoverEnter();
                    }
                }
                else
                {
                    _hoveredObject?.HandleHoverExit();
                    _hoveredObject = null;
                }
                
                if (Physics.Raycast(_ray, out RaycastHit propHit, Mathf.Infinity, propLayer))
                {
                    var propInteraction = propHit.transform.GetComponentInChildren<PropInteraction>();
                    if (_hoveredProp != propInteraction)
                    {
                        _hoveredProp?.HandleHoverExit();
                        _hoveredProp = propInteraction;
                        _hoveredProp?.HandleHoverEnter();
                    }
                }
                else
                {
                    _hoveredProp?.HandleHoverExit();
                    _hoveredProp = null;
                }
            }
            else
            {
                if (Physics.Raycast(_ray, out RaycastHit dropHit, Mathf.Infinity, _draggable.DropLayer))
                {
                    if (_lastDropHit != dropHit.transform)
                    {
                        _hoveredSlotHoverable?.HandleHoverExit();
                        _lastDropHit = dropHit.transform;

                        DropInteraction interaction = null;
                        if (dropHit.transform.TryGetComponent<PlayerSlot>(out var slot))
                            interaction = slot.DropInteraction;
                        else if (dropHit.transform.TryGetComponent<PlayerActionCard>(out var card))
                            interaction = card.DropInteraction;

                        _hoveredSlot = interaction;
                        _hoveredSlotHoverable = interaction;
                        _hoveredSlotHoverable?.HandleHoverEnter();
                    }
                }
                else
                {
                    _hoveredSlotHoverable?.HandleHoverExit();
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
            
            if (Physics.Raycast(clickRay, out RaycastHit cardHit, Mathf.Infinity, cardLayer))
            {
                AbstractCard card = cardHit.transform.GetComponent<AbstractCard>();
                if (card.IsUpDownMoving) return;

                _draggable = card.CardInteraction;
                float targetY = _draggable.Transform.position.y + dragUpOffset;
                Vector3 mouseWorldPos = GetCardTargetPos(targetY);

                _draggable.HandleDragStart(mouseWorldPos);
                _hoveredObject?.HandleHoverExit();
                _hoveredObject = null;

                _dragObjOriginLayer = _draggable.Transform.gameObject.layer;
                _draggable.Transform.gameObject.layer = draggingLayer;
                return;
            }

            if (Physics.Raycast(clickRay, out RaycastHit propHit, Mathf.Infinity, propLayer))
            {
                propHit.transform.GetComponentInChildren<PropInteraction>()?.HandleClick();
            }
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
                else if (dropHit.transform.TryGetComponent<PlayerActionCard>(out var card))
                    interaction = card.DropInteraction;

                interaction?.HandleDrop(_draggable.Transform);
                _hoveredSlotHoverable?.HandleHoverExit();
                _hoveredSlot = null;
                _hoveredSlotHoverable = null;
                _lastDropHit = null;
            }
            else
            {
                _draggable.HandleDragEnd();
            }

            _draggable.Transform.gameObject.layer = _dragObjOriginLayer;
            _draggable = null;
        }

        private void MoveMousePosition(Vector2 pos) => _mousePos = pos;

        private void MoveDraggable(Vector2 delta)
        {
            if (!_isDragging) return;
            float cardY = _draggable.Transform.position.y;
            Vector3 targetPos = GetCardTargetPos(cardY);
            _draggable.HandleDragging(targetPos);
        }

        private Vector3 GetCardTargetPos(float cardY)
        {
            Ray ray = Camera.main.ScreenPointToRay(_mousePos);
            Plane plane = new Plane(Vector3.up, new Vector3(0, cardY, 0));
            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);
            return _draggable.Transform.position;
        }
    }
}