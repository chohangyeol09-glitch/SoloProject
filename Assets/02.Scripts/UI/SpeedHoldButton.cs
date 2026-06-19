using System.Collections;
using _02.Scripts.CoreSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02.Scripts.UI
{
    /// <summary>
    /// 누르고 있는 동안 일정 간격으로 배속을 반복 적용하는 버튼.
    /// step을 양수로 두면 가속, 음수로 두면 감속 버튼.
    /// (누르는 즉시 1번 적용 → 이후 interval마다 반복)
    /// </summary>
    public class SpeedHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float step = 0.1f;       // 한 번에 적용할 양 (음수면 감속)
        [SerializeField] private float interval = 0.1f;   // 반복 간격(초)

        private Coroutine _routine;

        public void OnPointerDown(PointerEventData eventData)
        {
            Stop();
            _routine = StartCoroutine(HoldRoutine());
        }

        public void OnPointerUp(PointerEventData eventData) => Stop();
        public void OnPointerExit(PointerEventData eventData) => Stop();
        private void OnDisable() => Stop();

        private IEnumerator HoldRoutine()
        {
            while (true)
            {
                PresentationControl.AddSpeed(step);
                yield return new WaitForSeconds(interval);
            }
        }

        private void Stop()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }
        }
    }
}
