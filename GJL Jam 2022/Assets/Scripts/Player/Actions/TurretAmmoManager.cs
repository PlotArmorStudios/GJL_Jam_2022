using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretAmmoManager : AmmoManager
{
    public override void AddAmmo()
    {
        base.AddAmmo();
        // reload event goes here
        AkSoundEngine.PostEvent("Play_UI_Turret_Ready", gameObject);
    }
}
