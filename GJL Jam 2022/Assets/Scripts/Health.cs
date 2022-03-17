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
    
    protected float CurrentHealth { get; set; }
    protected float _changingHealth;

    //different types, depending on if it's a "temp" or "stationary" actor/thing
    public enum Type
    {
        Stationary,
        Timer
    };

    public Type HealthType;

    public void Start()
    {
        //simple set health to max value on initiation/setup of attached game object.
        CurrentHealth = _maxHealth; //can assign an int value to a float value
        _healthText.text = CurrentHealth + " / " + _maxHealth.ToString();
    }

    public virtual void TakeDamage(float damage)
    {
        _changingHealth = CurrentHealth;
        CurrentHealth -= damage;

        StartCoroutine(DepleteHPBar());

        Debug.Log(gameObject.name + " took damage.");

        if (CurrentHealth <= 0) Die();
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
            _healthBar.fillAmount -= .05f;

            _changingHealth = _healthBar.fillAmount * _maxHealth;
            
            _healthText.text = _changingHealth.ToString("0") + " / " + _maxHealth.ToString();
            
            yield return null;
        }
    }

    protected abstract void Die();
}