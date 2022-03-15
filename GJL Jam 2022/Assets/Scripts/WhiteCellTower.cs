using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCellTower : MonoBehaviour
{
    /* Code written by Andrew Letailleur, @ 15 March, 2022
 * Code edited by ______ ______, @ 15 March, 2022 */

    #region Summary & Credits

    /// <summary>
    /// The purpose of this script, is to act as the 'brains' and body of a tower defense
    /// IE: "Spawn pellets" at enemies, with the projectile script. Starting with the nearest one.
    /// And failing that, die/despawn after a set amount of time, 5 seconds by default.
    /// -
    /// As said, it also has a 'timed' limit on how long it should last in-game.
    /// By default, it's 5 seconds. But maybe, that time limit could be tripled/quadupled?
    /// </summary>

    /* References: Time.deltaTime, unity documents wise; under "per last frame" second wise
     * https://docs.unity3d.com/ScriptReference/Time-deltaTime.html
     * CreatePrimative, "generate game objects" out of thin air, for colliders/etc
     * https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html
     * GetChild, for transform lists. Game Object 'firing aim' should approx. Be in the last/2nd?
     * https://docs.unity3d.com/ScriptReference/Transform.GetChild.html
     * =====
     * Add Credits to key sources in this comment here, as/when needed*/

    #endregion

    //publlic variables, to be declared on top. Easy editing wise
        //lifespan timer variables
    public float LifespanTimer = 5.0f;//seconds of life. Consider triple/quadupling that timer, if required?
        //Only enable "_timer" if not doing a 'delayed' Destroy, optimization wise.

        //bullet/projectile variables
    public float BulletsPerSecond = 1.0f;//for "one bullet per second", tempo wise
    private float _bps_timer;//timer to set, dependant on the public "Bullets Per Second" value
    public float BulletVelocity = 15f;//tinker/taylor, until it is set properly

    //public for now, debug wise. But, need to figure out how to auto grab the 'key fire point/transform' of gun.
    public Transform _firePoint;//where to fire pellet from, instead of "center mass".

    //current target/AOE, depending on 'distance' to nearest enemy/boss per 'tick' check?
    private Transform _curTarget;//this should shift after a check, before firing at position wise.

    //to grab/serialize as an easy reference later, GameObject projectile wise
    private GameObject _bullet;//equals a "tiny sphere", to fire/fling at enemy. "Cell" wise...

    // Start is called before the first frame update
    void Start()
    {
            //first, initiate/setup timers
        Destroy(gameObject, LifespanTimer); //destroy on start/enable/spawn, 
        _bps_timer = 1.0f;//to represent 'a' second
        //_timer = MaxTimer; //only enable this if NOT, Destroy this after a public delay/etc.

        _firePoint = gameObject.transform.GetChild(2);//manually set to last index of White Cell tower.
            //Said manual is set to 2, under "turret_fire" gameobject, transform wise.

        CreatePrefabBullet();//for prototype referencing. Otherwise, _bullet should grab/reference a prefab.

        //TODO, "Assign new/white cell projectile Material to auto generated _bullet, at/upon start. 
    }

    private void CreatePrefabBullet() {
        //prototype setup, 'Bullet' prefab creation wise. Includes Mesh Filter and Render on CreatePrimative.
        _bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);//includes Mesh Filter and Render
        _bullet.AddComponent<Rigidbody>();//add rigidbody just to be sure, on _bullet prefab;
        _bullet.GetComponent<SphereCollider>().isTrigger = true;//just to ensure it 'phases' through rigidbodies, before disappearing.
        //        _bullet.AddComponent<???Projectile???>();//guessed variable, "in reference" to Projectile Script.

        //any other variables, reference here if/as needed. Prototype reference wise. :)
    }


    // Update is called once per frame
    private void Update()
    {
        //step A: Scan/assign target, to nearest enemy.
            //... Leave for 'firing at enemy', to call. Otherwise, keep aim at 'previous target'.
        //If previous target still active/alive, rotate to that previous 'target', aim wise
        if (_curTarget != null) //IE: IF there is a Targeted Enemy, adjust scope/aim.
        { //Consider making it "2.5D" Friendly, to bar 'rotating' turret glitch from target?
            Vector3 _sameAim = _curTarget.position;//debug 'fix' code, under being mindful to avoid a turret visual bug.
            _sameAim.y = gameObject.transform.position.y;//to keep the 'aim' of the singular turret on the same viewpoint.
            gameObject.transform.LookAt(_sameAim); //would be LookAt(_curTarget), IF the 'barrel/head' can rotate without issue
        }//added lines take into account, the 'turret glitch' if the entire model rotates 'upwards/downwards', visual wise.

        //step B: After X seconds? (Timer wise?), fire bullet. And do an enemy count/check, from those with the "Enemy" Tag/string.
        if (_bps_timer <= 0.0f) 
        {//only call TargetEnemy when required. NOT constantly, to 'save' on RAM/memory usage?
            TargetEnemy();// foreach inefficient. Could be called at occasionally?
            FireCell(); _bps_timer = 1.0f; 
        }//...after, a delay?
        else { _bps_timer -= (Time.deltaTime * BulletsPerSecond); }//deduct by firing rate, yo

        //Step C: Countdown/deprecate this Game Object, if enabled here past bullet fire code
            //not needed, under the lens of it was started at object start.
            //that said, IF mindful on pause menu. Consider implementing reference here, to make spawn length 'pause safe'.
        /*        _timer -= Time.deltaTime //timercode. Only enable if there's no timed Destroy at Start.
        //        if (_timer <= 0.0f) Destroy(gameObject);//alternate delete, if 
        *////endif
    }

    //counts the amount of enemies with the "Enemy" tag (not boss), iteratively setting aim to the nearest enemy to the turret.
    private void TargetEnemy()
    {//ref: https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
        _curTarget = null;
        GameObject[] _foes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject _foe in _foes)
        {
            if (_curTarget = null)
            { _curTarget = _foe.transform; }
            else
            {//compare by (a-b).magnitute, transform.position wise
                float aDist = Vector3.Distance(
                    gameObject.transform.position,
                    _curTarget.transform.position);
                float bDist = Vector3.Distance(
                    gameObject.transform.position,
                    _foe.transform.position);
                if (bDist < aDist)
                { _curTarget = _foe.transform; }
                //compare distance between _foe.transform, and curTarget.transform
                //... to the White Cell tower. Ref; https://iqcode.com/code/typescript/how-to-compare-distance-between-2-objects-unity
            }//endif
        }//end foreach loop
    }//end TargetEnemy

    //fire a projectile, from the "White Cell tower", defense bullet wise.
    private void FireCell() {
        //hack rotationary, 'flip back' edition
        Quaternion _fireRotate = _firePoint.transform.rotation;
        _fireRotate.z -= 90f;//aim adjustment from 'end barrel' stroke, hack wise.
            //Just so that red arrow points forward. Fix/[REDACT] this, if model or aim point's fixed.
        //consider an added transform from the 'aim' here, if projectile test bugs upon spawning.
        GameObject go = Instantiate(_bullet, _firePoint.transform.position, _fireRotate);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        //depending on scale, times it by a factor of 1/10/100?
        rb.AddForce(transform.right * BulletVelocity);
        //rb.AddTorque, if wanting cell to 'spin' at random?
//        Debug.LogWarning("Object is spawned at: " + go.transform.position + ", Time Spawned = " + Time.fixedTime);
    }


}
