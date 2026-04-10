using UnityEngine;

namespace _02.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaApplier : MonoBehaviour
    {
        private RectTransform _panel;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            ApplySafeArea();
        }
        
        private void OnRectTransformDimensionsChanged()
        {
            if (_panel != null)
                ApplySafeArea();
        }

        void ApplySafeArea()
        {
            Rect safeZone = Screen.safeArea;

            Vector2 minAnchor = safeZone.position;
            Vector2 maxAnchor = safeZone.position + safeZone.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            _panel.anchorMin = minAnchor;
            _panel.anchorMax = maxAnchor;

        }
    }
}