#define PlayerHealthDebug
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerHealth : Health
{
    public static event Action<float> OnDamageKidney;
    [SerializeField] private float _kidneyDamage = 30f;

    [ContextMenu("Take Damage Test")]
    private void TakeDamageTestMethod()
    {
        TakeDamage(50);
    }
    
    protected override void Die()
    {
        //Kidney takes damage when player dies
        OnDamageKidney?.Invoke(_kidneyDamage);

        #region DebugLogs

#if PlayerHealthDebug
        Debug.Log("Player death did damage to kidney.");
#endif

        #endregion
    }
}