using UnityEngine;

namespace _02.Scripts.DragSystem
{
    public interface IDraggable
    {
        public Transform Transform { get; }
        
        public void OnDragStart();
        public void OnDragging(Vector3 pos);
        public void OnDragEnd();
    }
}