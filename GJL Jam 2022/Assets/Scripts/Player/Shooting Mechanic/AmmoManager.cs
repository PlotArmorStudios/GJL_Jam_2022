using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] protected List<RectTransform> _ammoUI;
    [SerializeField] protected int _maxAmmo = 4;
    
    protected int _currentDamageAmmo;
    
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


    public void AddAmmo()
    {
        _currentDamageAmmo++;
        RefreshUI();
    }
    
    private void RefreshUI()
    {
        foreach (var ammoUI in _ammoUI)
        {
            ammoUI.gameObject.SetActive(false);
        }

        for (int i = 0; i < _currentDamageAmmo; i++)
        {
            _ammoUI[i].gameObject.SetActive(true);
        }
    }
}