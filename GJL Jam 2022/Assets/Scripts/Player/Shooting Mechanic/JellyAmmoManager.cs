using UnityEngine;

public class JellyAmmoManager : AmmoManager
{
    private JellyShotToggler _jellyShotToggler;

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
}