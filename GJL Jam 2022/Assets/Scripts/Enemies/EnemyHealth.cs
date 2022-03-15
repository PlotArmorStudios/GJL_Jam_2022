using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{   
    public event Action _onDie;
    public void ResetHealth()
    {
        CurrentHealth = _maxHealth;
    }

    public void SetNewMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    protected override void Die()
    {
        _onDie.Invoke();
    }
}