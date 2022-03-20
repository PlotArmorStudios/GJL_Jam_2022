using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : Health
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger("Take Damage");
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        _animator.SetTrigger("Die");
        GameManager.Instance.WinGame();
    }
}