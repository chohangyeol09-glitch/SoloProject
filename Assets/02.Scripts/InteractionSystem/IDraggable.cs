using UnityEngine;

namespace _02.Scripts.InteractionSystem
{
    public interface IDraggable
    {
        Transform Transform { get; }
        LayerMask DropLayer { get; }

        void HandleDragStart(Vector3 mouseWorldPos);
        void HandleDragging(Vector3 pos);
        void HandleDragEnd();
    }
}