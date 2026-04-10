using _02.Scripts.ModuleSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02.Scripts.UI
{
    public class UIDrag : MonoBehaviour, IModule, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private string dropFieldTag;
        [SerializeField] private GameObject cardPrefab;
        
        private ModuleOwner _owner;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private Vector2 _moveStartPos;
        
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            Debug.Assert(_canvasGroup != null, $"{gameObject.name}: canvas group is null");
            Debug.Assert(_canvas != null, $"{gameObject.name}: canvas is null");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("StartDrag");
            transform.SetAsLastSibling();
            _canvasGroup.blocksRaycasts = false;
            _moveStartPos = _rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            if (IsOnDropField(eventData))
            {
                //임시 생성, 나중에 분리하기.
                Vector3 worldPos = WorldAndScreenChange.ScreenToWorld(eventData.position, Camera.main);
                Instantiate(cardPrefab, worldPos, Quaternion.identity);
                Destroy(gameObject);
            }
            else
                _rectTransform.anchoredPosition = _moveStartPos;
        }

        public bool IsOnDropField(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.tag);
                Debug.Log(hit.collider.name);
                
                return hit.collider.CompareTag(dropFieldTag);
            }

            return false;
        }
    }
}