using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Code written by Andrew Letailleur, @ 14 March, 2022
 * Code edited by Khenan Newton, @ 14 March, 2022 */

#region Summary & Credits

/// <summary>
/// The purpose of this script, is to act as a general health ref,
/// For all sorts of actors. Be they Player, Enemy, Base/Kidney.
/// Or heck, even "WhiteCell Tower", if 'health' ~/~ Time for them.
/// -
/// Depending on how things go. Could/should ideally, used as reference
/// For the "Kidney" or "Player" script, if either lose all Health
/// and 'disappear' as a result, before revive/game over is taken into account.
/// </summary>
/*Add Credits/references to key sources in this comment here*/

#endregion

public abstract class Health : MonoBehaviour
{
    //base Health variables, for referencing.
    [SerializeField] protected int _maxHealth = 100; //public reference, for easy User-Interface editing
    [SerializeField] protected Image _healthBar;
    [SerializeField] protected TMP_Text _healthText;
    
    public bool IsAlive { get; set; }
    
    protected float CurrentHealth { get; set; }
    protected float _changingHealth;

    //different types, depending on if it's a "temp" or "stationary" actor/thing
    public enum Type
    {
        Stationary,
        Timer
    }

    public Type HealthType;

    protected virtual void OnEnable()
    {
        //simple set health to max value on initiation/setup of attached game object.
        CurrentHealth = _maxHealth; //can assign an int value to a float value
        
        Debug.Log(gameObject.name + "Max hp is: " + _maxHealth);
        Debug.Log(gameObject.name + "Current hp is: " + CurrentHealth);
 
        _healthBar.fillAmount = CurrentHealth / _maxHealth;
        _healthText.text = CurrentHealth + " / " + _maxHealth.ToString();
        IsAlive = true;
    }

    public virtual void TakeDamage(float damage)
    {
        _changingHealth = CurrentHealth;
        CurrentHealth -= damage;

        StartCoroutine(DepleteHPBar());

        if (CurrentHealth <= 0 && IsAlive)
        {
            Debug.Log("Current health depleted");
            IsAlive = false;
            Die();
        }
    }


    [ContextMenu("Take Damage Test")]
    private void TakeDamageTestMethod()
    {
        TakeDamage(50);
    }

    protected virtual IEnumerator DepleteHPBar()
    {
        //yield return _healthBar.fillAmount != CurrentHealth / _maxHealth;
        while (_healthBar.fillAmount > CurrentHealth / _maxHealth)
        {
            _healthBar.fillAmount -= .01f;

            _changingHealth = _healthBar.fillAmount * _maxHealth;
            
            _healthText.text = _changingHealth.ToString("0") + " / " + _maxHealth.ToString();
            
            yield return null;
        }
    }

    [ContextMenu("Die")]
    protected abstract void Die();
}