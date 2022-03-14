using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _kidneyDamage = 30f;
    private KidneyPlaceHolder _kidney;
    private PlayerControl _player;
    private float _health;

    private void Start()
    {
        _player = GetComponent<PlayerControl>();
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Kidney takes damage when player dies
        _kidney.Health.TakeDamage(_kidneyDamage);
        GameManager.Instance.DespawnPlayer();
        //Reswawn player if Kidney is still alive
        if (_kidney.Health.CurrentHealth > 0)
            RespawnPlayer();
    }

    private void RespawnPlayer()
    {
    }
}

public class KidneyPlaceHolder : MonoBehaviour
{
    private KidneyHealthPlaceHolder _healthPlaceHolder;
    public KidneyHealthPlaceHolder Health => _healthPlaceHolder;
}

public class KidneyHealthPlaceHolder
{
    private float _maxHealth = 100f;
    public float CurrentHealth { get; set; }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.LoseGame();
    }
}