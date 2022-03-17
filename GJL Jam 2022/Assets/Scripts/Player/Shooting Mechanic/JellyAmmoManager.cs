using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JellyAmmoManager : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _damageShotUI;
    [SerializeField] private int _maxAmmo = 4;
    
    private int _currentDamageAmmo;
    
    public int CurrentDamageAmmo => _currentDamageAmmo;

    private void Start()
    {
        _currentDamageAmmo = _maxAmmo;
        RefreshUI();
    }

    public void SubtractAmmo()
    {
        _currentDamageAmmo--;
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (var ammoUI in _damageShotUI)
        {
            ammoUI.gameObject.SetActive(false);
        }

        for (int i = 0; i < _currentDamageAmmo; i++)
        {
            _damageShotUI[i].gameObject.SetActive(true);
        }
    }

    public void AddAmmo()
    {
        _currentDamageAmmo++;
        RefreshUI();
    }
}