#define DebugKidneyHP
using System;
using UnityEngine;

public class KidneyHealth : Health
{
    private void OnEnable()
    {
        PlayerHealth.OnDamageKidney += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDamageKidney -= TakeDamage;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        #region DebugLogs

#if DebugKidneyHP

        Debug.Log("Kidney took damage from player");
#endif

        #endregion

        //player will still respawn even after kidney dies
        GameManager.Instance.DespawnPlayer();
    }

    protected override void Die()
    {
        GameManager.Instance.LoseGame();
    }
}