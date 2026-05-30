using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.DragSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.CardSystem
{
    public abstract class AbstractCard : ModuleOwner, IClickable, IDraggable, IHoverable
    {
        public Transform Transform => transform;
        
        [SerializeField] private Image cardImage;
        
        public virtual void OnClick()
        {
            Debug.Log("카드 클릭: " + gameObject.name);
        }

        public virtual void OnDragStart()
        {
            Debug.Log("카드 드래그 시작: " + gameObject.name);
        }

        public virtual void OnDragging(Vector3 pos)
        {
            transform.position = pos;
        }

        public virtual void OnDragEnd()
        {
            Debug.Log("카드 드래그 끝: " + gameObject.name);
        }

        public virtual void OnHoverEnter()
        {
            Debug.Log("카드 호버링 시작: " + gameObject.name);
        }

        public virtual void OnHoverExit()
        {
            Debug.Log("카드 호버링 끝: " + gameObject.name);
        }
    }
}
