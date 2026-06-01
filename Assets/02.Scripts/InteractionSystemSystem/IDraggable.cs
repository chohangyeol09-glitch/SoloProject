using UnityEngine;

namespace _02.Scripts.InteractionSystemSystem
{
    public interface IDraggable
    {
        public Transform Transform { get; }
        public LayerMask DropLayer { get; }
        
        public void OnDragStart();
        public void OnDragging(Vector3 pos);
        public void OnDragEnd();
    }
}