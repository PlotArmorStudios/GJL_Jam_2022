using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JellyAmmoManager : MonoBehaviour
{
    [SerializeField] private RectTransform _damageShotUI;
    [SerializeField] private int _maxAmmo = 4;
    
    private int _currentDamageAmmo;
    
    private List<RectTransform> _jellyShotUI;

    public int CurrentDamageAmmo => _currentDamageAmmo;

    private void Start()
    {
        _currentDamageAmmo = _maxAmmo;
        _jellyShotUI = _damageShotUI.GetComponentsInChildren<RectTransform>(true).ToList();
        RefreshUI();
    }

    public void SubtractAmmo()
    {
        _currentDamageAmmo--;
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (var ammoUI in _jellyShotUI)
        {
            ammoUI.gameObject.SetActive(false);
        }

        for (int i = 0; i <= _currentDamageAmmo; i++)
        {
            _jellyShotUI[i].gameObject.SetActive(true);
        }
    }

    public void AddAmmo()
    {
        _currentDamageAmmo++;
        RefreshUI();
    }
}