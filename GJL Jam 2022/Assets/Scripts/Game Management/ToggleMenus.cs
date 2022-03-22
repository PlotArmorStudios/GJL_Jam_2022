using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenus : MonoBehaviour
{
    [SerializeField] private GameObject _hudUI;
    [SerializeField] private GameObject _pauseMenu;

    private void OnEnable()
    {
        GameManager.OnGamePause += ToggleMenuOn;
        GameManager.OnGameUnpause += ToggleMenuOff;
        GameManager.OnFlashTutorial += ToggleHUD;
    }
    private void OnDisable()
    {
        GameManager.OnGamePause -= ToggleMenuOn;
        GameManager.OnGameUnpause -= ToggleMenuOff;
        GameManager.OnFlashTutorial -= ToggleHUD;
    }

    private void ToggleMenuOff()
    {
        _pauseMenu.gameObject.SetActive(false);
        _hudUI.gameObject.SetActive(true);
    }

    private void ToggleMenuOn()
    {
        _pauseMenu.gameObject.SetActive(true);
        _hudUI.gameObject.SetActive(false);
    }

    private void ToggleHUD()
    {
        _hudUI.gameObject.SetActive(false);
    }
}
