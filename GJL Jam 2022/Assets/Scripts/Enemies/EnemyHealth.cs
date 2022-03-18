/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{   
    public event Action OnDie;
    
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
        OnDie?.Invoke();
    }
}
