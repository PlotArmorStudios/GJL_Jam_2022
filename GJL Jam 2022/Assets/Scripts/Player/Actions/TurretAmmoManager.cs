using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Intentionally left blank to house Turret ammo manager specific variables
public class TurretAmmoManager : AmmoManager
{
    public override void AddAmmo()
    {
        base.AddAmmo();
        // reload event goes here
        Debug.Log("Play turret reload sound");

        AkSoundEngine.PostEvent("Play_UI_Turret_Ready", gameObject);
    }
}