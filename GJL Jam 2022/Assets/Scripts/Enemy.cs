using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHealth;
    private int _currHealth;

    public void TakeDamage(int damage)
    {
        _currHealth -= damage;
        if (_currHealth <= 0) Die();
    }

    public void ResetHealth()
    {
        _currHealth = _maxHealth;
    }

    protected abstract void Die();
}
