using System;
using UnityEngine;

namespace _02.Scripts.CoreSystem
{
    public static class PresentationControl
    {
        public const string GameplayId = "gameplay";   

        private static int _busyCount;
        public static bool IsBusy => _busyCount > 0;

        
        public static float Speed { get; private set; } = 1f;
        public static event Action<float> OnSpeedChanged;

        public static void SetSpeed(float speed)
        {
            speed = Mathf.Clamp(speed, 0.5f, 3f);
            if (Mathf.Approximately(speed, Speed)) return;   // 변화 없으면 무시
            Speed = speed;
            OnSpeedChanged?.Invoke(Speed);                    // ← 구독자(UI)에게 알림
        }
        public static void AddSpeed(float speed) => SetSpeed(Speed + speed);
        public static void CycleSpeed() => SetSpeed(Speed >= 2f ? 1f : 2f);  

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetState()
        {
            _busyCount = 0;
            Speed = 1f;
            OnSpeedChanged = null; 
        }

        public static IDisposable Busy()
        {
            _busyCount++;
            return new BusyScope();
        }

        private static void Release() => _busyCount = Mathf.Max(0, _busyCount - 1);

        private class BusyScope : IDisposable
        {
            private bool _disposed;
            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;
                Release();
            }
        }
    }
}
