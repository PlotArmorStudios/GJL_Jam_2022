#define DebugKidneyHP
using System;
using UnityEngine;

public class KidneyHealth : Health
{
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerHealth.OnPlayerDeath += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath += TakeDamage;
    }

    protected override void Die()
    {
        GameManager.Instance.LoseGame();
    }
}