using System;
using UnityEngine;

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