using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _camMainMenu;
    [SerializeField] private CinemachineVirtualCamera _camGame;

    public void SwitchCamera(bool toCamGame)
    {
        if (toCamGame)
        {
            _camMainMenu.Priority = 0;
            _camGame.Priority = 10;
        }
        else
        {
            _camMainMenu.Priority = 10;
            _camGame.Priority = 0;
        }
    }
}
