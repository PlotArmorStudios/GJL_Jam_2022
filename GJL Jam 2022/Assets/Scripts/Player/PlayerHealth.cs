#define PlayerHealthDebug

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public static event Action<float> OnPlayerDeath;
    [SerializeField] private float _kidneyDamage = 30f;

    protected override void Die()
    {
        //Kidney takes damage when player dies
        OnPlayerDeath?.Invoke(_kidneyDamage);
        //player will still respawn even after kidney dies
        GameManager.Instance.DespawnPlayer();

        #region DebugLogs

#if PlayerHealthDebug
        Debug.Log("Player death did damage to kidney.");
#endif

        #endregion
    }
}