using _02.Scripts.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.UI
{
    public static class WorldAndScreenChange
    {
        public static Vector3 ScreenToWorld(Vector2 position, Camera camera, float groundY = 0f) 
        {
            Ray ray = camera.ScreenPointToRay(position);
            
            Plane plane = new Plane(Vector3.up, new Vector3(0, groundY, 0));
            
            if (plane.Raycast(ray, out float distance))
                return ray.GetPoint(distance);
            
            return Vector3.zero;
        }

        public static Vector2 WorldToScreen(Vector3 position, Camera camera)
        {
            return camera.ScreenToWorldPoint(position);
        }
    }
}
