using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Health : MonoBehaviour
{
    //base Health variables, for referencing.
    public int _maxHealth = 100;//public reference, for easy User-Interface editing
    private float _currentHealth;//actual code, for gameplay reference purposes.
    public bool isAlive = true;//set to true, until if/when _currentHealth <= 0

    //different types, depending on if it's a "temp" or "stationary" actor/thing
    public enum Type { Stationary, Timer}; public Type HealthType;

    public void Start()
    {//simple set health to max value on initiation/setup of attached game object.
        _currentHealth = _maxHealth;//can assign an int value to a float value
    }//end Start

    //deducts current health by a set 'tick' per update. Ideally, through Time.deltaTime;
    //Note by Khenan - This will be a Kidney specific feature. Leave this for a child class 'KidneyHealth'
    private void HealthTimer() 
    {//leave it as a public option, for "alternate tick damage"
        _currentHealth -= Time.deltaTime;//the "tick"
            //invoked by update call instead?
        //if (_currentHealth <= 0.0f) { isAlive = false; _endState(); }
    }//end of HealthTimer

    //public damage method, to be invoked by damaging projectile/foe
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        //will check if died only when taking damage
        if (_currentHealth <= 0 && isAlive)
        { 
            isAlive = false;
            Die();
        }
    }

    private void Die() {
        //call 'last functions' attached to this scripts Game Object.
        //Before Destroy/disable game object is used, object pool wise.
        Debug.Log("Actor possessing Health Type " +
            HealthType.ToString() + "Is down.");
        //Destroy(gameObject);
    }

}
