#define DebugKidneyHP
using System;
using UnityEngine;

public class KidneyHealth : Health
{
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerHealth.OnPlayerDeath += TakeDamage;
        Boss.OnBossDamage += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath += TakeDamage;
        Boss.OnBossDamage -= TakeDamage;
    }

    protected override void Die()
    {
        GameManager.Instance.LoseGame();
    }
}