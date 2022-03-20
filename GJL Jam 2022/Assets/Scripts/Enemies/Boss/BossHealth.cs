using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : Health
{
    public static event Action<float> OnTakeDamage;

    public override void TakeDamage(float damage)
    {
        
        base.TakeDamage(damage);
        OnTakeDamage?.Invoke(CurrentHealth / _maxHealth);
    }
    protected override void Die()
    {
        Debug.Log("Boss died");
        GameManager.Instance.WinGame();
    }
}
