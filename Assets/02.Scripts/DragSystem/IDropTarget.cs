using UnityEngine;

namespace _02.Scripts.DragSystem
{
    public interface IDropTarget
    {
        public void OnDrop(Transform dropTrm);
    }
}