using System;
using _02.Scripts.CoreSystem;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeCamera : MonoSingleton<ChangeCamera>
{
    [SerializeField] private CinemachineCamera topViewCamera;
    [SerializeField] private CinemachineCamera bottomViewCamera;
    [SerializeField] private CinemachineCamera settingViewCamera;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            bottomViewCamera.Priority = 10;
            topViewCamera.Priority = 0;
            settingViewCamera.Priority = 0;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            bottomViewCamera.Priority = 0;
            topViewCamera.Priority = 10;
            settingViewCamera.Priority = 0;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            bottomViewCamera.Priority = 0;
            topViewCamera.Priority = 0;
            settingViewCamera.Priority = 10;
        }
    }

    public void SetTopView()
    {
        bottomViewCamera.Priority = 0;
        topViewCamera.Priority = 10;
        settingViewCamera.Priority = 0;
    }

    public void RestoreView()
    {
        bottomViewCamera.Priority = 10;
        topViewCamera.Priority = 0;
        settingViewCamera.Priority = 0;
    }
}
