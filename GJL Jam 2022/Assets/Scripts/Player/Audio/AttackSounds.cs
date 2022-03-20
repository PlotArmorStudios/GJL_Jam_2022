using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSounds : MonoBehaviour
{
    private JellyShotToggler _jellyShotToggler;

    private void Start()
    {
        _jellyShotToggler = GetComponentInParent<JellyShotToggler>();
    }

    private void PlayAttackSound()
    {
        if (_jellyShotToggler.JellyToShoot == JellyToShoot.FreezingJelly)
            AkSoundEngine.PostEvent("Play_Freeze_Shot", gameObject);
        if (_jellyShotToggler.JellyToShoot == JellyToShoot.DamageJelly)
        {
            //Play Damage shot
            Debug.Log("Play Damage Shot");
            AkSoundEngine.PostEvent("Play_Damage_Projectile", gameObject);
        }
    }
}