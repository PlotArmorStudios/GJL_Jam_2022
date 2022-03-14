using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Code written by Andrew Letailleur, @ 14 March, 2022
 * Code edited by _____ _____, @ 14? March?, 2022 */


public class HealthRef : MonoBehaviour
{
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

    //base Health variables, for referencing.
    public int InitialHealth = 100;//public reference, for easy User-Interface editing
    private float _currentHealth;//actual code, for gameplay reference purposes.
    public bool isAlive = true;//set to true, until if/when _currentHealth <= 0

    //different types, depending on if it's a "temp" or "stationary" actor/thing
    public enum Type { Stat, Timer}; public Type HealthType;

    // Start is called before the first frame update
    public void Start()
    {//simple set health to max value on initiation/setup of attached game object.
        _currentHealth = InitialHealth;//can assign an int value to a float value
    }//end Start

    //maybe later, "invoke set health" as a method? But possibly not enable, to avoid cheating?

    // Update is called once per frame, for internal update purposes. To FixedUpdate?
    public void Update()
    {//simple Health checks, per frame. Consider disabling to optimise run time later
        if (_currentHealth <= 0 && isAlive)
        { 
            isAlive = false;
            _endState();
        }//end if, end object check

        //conditional 'internal health drain' check
        if (HealthType == Type.Timer) { _healthTimer(); }
    }//end of Update

    //deducts current health by a set 'tick' per update. Ideally, through Time.deltaTime;
    private void _healthTimer()
    {//leave it as a public option, for "alternate tick damage"
        _currentHealth -= Time.deltaTime;//the "tick"
            //invoked by update call instead?
        //if (_currentHealth <= 0.0f) { isAlive = false; _endState(); }
    }//end of HealthTimer

    //public damage script, to be invoked by damaging projectile/foe
    public float HealthCount(float DMG)
    {
        _currentHealth -= DMG;
            //invoked by update call instead?
        //if (_currentHealth <= 0.0f) { isAlive = false; _endState(); }
        return _currentHealth;//to parse/reference, for callback scripts.
    }

    private void _endState() {
        //call 'last functions' attached to this scripts Game Object.
        //Before Destroy/disable game object is used, object pool wise.
        Debug.Log("Actor possessing Health Type " +
            HealthType.ToString() + "Is down.");
        //Destroy(gameObject);
    }

}
