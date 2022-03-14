using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;//disabled for now, but might be groovy later
    //using UnityEngine.UI;// double disabled. Under uncertainty over needing UI, and/or UI Elements

/* Code written by Andrew Letailleur, @ 14 March, 2022
 * Code edited by _____ _____, @ 14? March?, 2022 */

public class KidneyBase : MonoBehaviour
{
    #region Summary & Credits
    /// <summary>
    /// The purpose of this script, is to invoke health deduction on triggered events.
    /// Mainly, 'trigger' health loss in case of damage, or player revive.
    /// And set/trigger a game over scene, or flag when health reaches 0%/zero value.
    /// -
    /// PS: UnityEngine.SceneManagement is under reference, but disabled
    /// Due to uncertainty on if it'd be invoked by this script, or on the GUI/UI Elements panel/layer.
    /// </summary>

    /*Add Credits/references to key sources in this comment here*/
    #endregion

    public Transform RespawnPos;//lazy ref example, for 'checkpoint' position
    private GameObject _playerRef;//GameObject... Or is it?
    private Health _healthScript;//default should be 100, and already attached
        //health ref. "Default" is 100. So scale based on that?

        //revive mechanic stats, to deduct current HP from.
    public int ReviveCost = 30; public int CostScale = 10;
    public int SetCooldown = 5; private float _timer;
    //timer base, for "player revive" calc?


    // Start is called before the first frame update
    void Start()
    {
        //for the Player singleton reference tag system wise
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        //health ref, attached to this game object
        _healthScript = gameObject.GetComponent<Health>();
        if (_healthScript == null) {
            gameObject.AddComponent<Health>();//add a new health script, improv wise
            Debug.LogWarning("No Health Script attached on start, adding default values");
        }//endif

    }

    //variables to call on Kidney, if player is dead, and to revive
    public void PlayerDied() {
        _timer = SetCooldown; _playerRef = null;//empty reference, until respawned
    }
    private void _revivePlayer() {//tdl, add Game Over call/check
        //... after timer?/countdown?
        _healthScript.TakeDamage(ReviveCost);
            //cheap cost-scale math.
        ReviveCost += CostScale; CostScale *= 2;
            // Ensure above formula is in line with designed life cost later

    }

    // Update is called once per frame
    void Update()
    {
        //more efficient than calling "Find Player Object all the time

        if (_playerRef == null)
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {//countdown
                _revivePlayer();
            }//endif
        }//endif

        
    }//end of Update




}
