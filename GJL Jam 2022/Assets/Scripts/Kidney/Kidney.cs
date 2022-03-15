using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;//disabled for now, but might be groovy later
    //using UnityEngine.UI;// double disabled. Under uncertainty over needing UI, and/or UI Elements

/* Code written by Andrew Letailleur, @ 14 March, 2022
 * Code edited by _____ _____, @ 14? March?, 2022 */
[RequireComponent(typeof(KidneyHealth))]
public class Kidney : MonoBehaviour
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

    private KidneyHealth _health;//default should be 100, and already attached
    public KidneyHealth Health => _health;

    void Start()
    {
        //health ref, attached to this game object
        _health = gameObject.GetComponent<KidneyHealth>();
    }
}
