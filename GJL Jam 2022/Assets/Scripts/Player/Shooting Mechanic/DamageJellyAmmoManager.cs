using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageJellyAmmoManager : AmmoManager
{
    [SerializeField] private int _requiredAmmoStep = 2;
    private JellyShotToggler _jellyShotToggler;
    private int _ammoStep;

    protected override void Start()
    {
        base.Start();
        _jellyShotToggler = GetComponent<JellyShotToggler>();
    }


    public override void SubtractAmmo()
    {
        if (_jellyShotToggler.JellyToShoot == JellyToShoot.DamageJelly)
        {
            base.SubtractAmmo();
        }
    }

    private IEnumerator FadeAmmo(Image image)
    {
        image.color = new Color(1, 1, 1, .3f);
        yield break;
    }

    private IEnumerator BrightenAmmoUI(Image image)
    {
        image.color = new Color(1, 1, 1, 1f);
        yield break;
    }
    
    protected override void RefreshUI()
    {
        foreach (var ammoUI in _ammoUI)
        {
            var ammoImage = ammoUI.GetComponent<Image>();
            StartCoroutine(FadeAmmo(ammoImage));
        }

        for (int i = 0; i < CurrentAmmo; i++)
        {
            var ammoImage = _ammoUI[i].GetComponent<Image>();
            StartCoroutine(BrightenAmmoUI(ammoImage));
        }
    }
    
    public override void AddAmmo()
    {
        if (CurrentAmmo < _maxAmmo)
            _ammoStep++;

        if (_ammoStep >= _requiredAmmoStep)
        {
            base.AddAmmo();
            _ammoStep = 0;
        }
    }
}