#define PlayerHealthDebug

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public static event Action<float> OnPlayerDeath;
    [SerializeField] private float _kidneyDamage = 30f;
    [SerializeField] private Image _secondHPBar;

    protected override IEnumerator DepleteHPBar()
    {
        while (_healthBar.fillAmount > CurrentHealth / _maxHealth)
        {
            _healthBar.fillAmount -= .05f;
            _secondHPBar.fillAmount = _healthBar.fillAmount;

            _changingHealth = _healthBar.fillAmount * _maxHealth;
            _healthText.text = CurrentHealth.ToString("0") + " / " + _maxHealth.ToString();
            
            yield return null;
        }
    }

    protected override void Die()
    {
        //Kidney takes damage when player dies
        OnPlayerDeath?.Invoke(_kidneyDamage);
        //player will still respawn even after kidney dies
        GameManager.Instance.DespawnPlayer();

        #region DebugLogs

#if PlayerHealthDebug
        Debug.Log("Player death did damage to kidney.");
#endif

        #endregion
    }
}