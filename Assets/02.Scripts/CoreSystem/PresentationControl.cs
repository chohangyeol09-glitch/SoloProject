using System;
using UnityEngine;

namespace _02.Scripts.CoreSystem
{
    public static class PresentationControl
    {
        public const string GameplayId = "gameplay";   

        private static int _busyCount;
        public static bool IsBusy => _busyCount > 0;

        [Range(0.5f,3)]
        public static float Speed { get; private set; } = 1f;
        public static void SetSpeed(float speed) => Speed = Mathf.Clamp(speed, 0.5f, 3);
        public static void AddSpeed(float speed) => SetSpeed(Speed += speed);
        public static void CycleSpeed() => SetSpeed(Speed >= 2f ? 1f : 2f);  

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetState()
        {
            _busyCount = 0;
            Speed = 1f;
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
