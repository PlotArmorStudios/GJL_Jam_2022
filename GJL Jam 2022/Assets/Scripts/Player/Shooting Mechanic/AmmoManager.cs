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

    public int CurrentAmmo { get; set; }


    private void OnEnable()
    {
        AddToAmmo.OnMinionFreeze += AddAmmo;
    }

    protected virtual void Start()
    {
        CurrentAmmo = _maxAmmo;
        RefreshUI();
    }

    public virtual void SubtractAmmo()
    {
        CurrentAmmo--;
        RefreshUI();
    }


    public virtual void AddAmmo()
    {
        if (CurrentAmmo < _maxAmmo)
            CurrentAmmo++;
        RefreshUI();
    }

    protected void RefreshUI()
    {
        foreach (var ammoUI in _ammoUI)
        {
            ammoUI.gameObject.SetActive(false);
        }

        for (int i = 0; i < CurrentAmmo; i++)
        {
            _ammoUI[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        AddToAmmo.OnMinionFreeze -= AddAmmo;
    }
}