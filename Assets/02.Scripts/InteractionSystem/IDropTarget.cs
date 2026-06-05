using UnityEngine;

namespace _02.Scripts.InteractionSystem
{
    public interface IDropTarget
    {
        public void HandleDrop(Transform dropTrm);
    }
}