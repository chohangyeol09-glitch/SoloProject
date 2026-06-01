using UnityEngine;

namespace _02.Scripts.InteractionSystemSystem
{
    public interface IDropTarget
    {
        public void HandleDrop(Transform dropTrm);
    }
}