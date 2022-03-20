using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for manual hack on "hey, child of child"

public class WhiteCellTower : MonoBehaviour
{
    /* Code written by Andrew Letailleur, @ 15 March, 2022
 * Code edited by Andrew Letailleur, @ 19 March, 2022 */

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
     * Quaternion.Slerp, 'smoother rotation' try edition
     * https://forum.unity.com/threads/lookat-slerp.63353/
     * =====
     * Add Credits to key sources in this comment here, as/when needed*/

    #endregion

    //Future/Temp notes
    ///Figuire out how to 'spawn aim' away from player, on Awake/Start of this object, yo

    //publlic variables, to be declared on top. Easy editing wise
    //lifespan timer variables
    public float LifespanTimer = 5.0f; //seconds of life. Consider triple/quadupling that timer, if required?

    private float _timer, _setMaxTimer; //enable, as 'GUI' needs to show before disabling/destroying itself, timer wise.
    private Image _timerGUI; //last minute addition, to update/calc 'fill' from compared

    //bullet/projectile variables
    public float BulletsPerSecond = 1.0f; //for "one bullet per second", tempo wise
    private float _bps_timer; //timer to set, dependant on the public "Bullets Per Second" value
    public float BulletVelocity = 15f; //not-so deprecated, as projectile prefab has it's internal velocity setup.

    //all set up, private references wise. May need adjustments, if attached to a different prefab.
    private GameObject _turretSpark; //to maybe rapidly enable/disable, upon spawn.

    private Transform _firePoint; //where to fire pellet from, instead of "center mass".
    //current target/AOE, depending on 'distance' to nearest enemy/boss per 'tick' check?
    //private Transform _curTarget;//this should shift after a check, before firing at position wise.

    //Public for now. Need to grab/serialize as an easy reference later, GameObject projectile wise. Until then, it's public
    public GameObject Bullet; //equals a "tiny sphere", to fire/fling at enemy. "Cell" wise...
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        //first, initiate/setup timers

        _animator = GetComponent<Animator>();
        _bps_timer = 1.0f; //to represent 'a' second
        _timer = LifespanTimer;
        _setMaxTimer = LifespanTimer; //only enable this if NOT, Destroy this after a public delay/etc.
        //Destroy(gameObject, LifespanTimer); //destroy on start/enable/spawn, //DONOT enable until testing is done

        //manually set to key indexes of White Cell tower.
        _turretSpark = gameObject.transform.GetChild(2).gameObject;
        _firePoint = gameObject.transform.GetChild(3).gameObject.transform;
        _timerGUI = gameObject.transform.GetChild(4).GetChild(1).GetComponent<Image>(); //UI addition
        //Debug.LogWarning("Image CHECK. Image is " + _timerGUI.name);

//        BulletVelocity = Bullet.GetComponent<Projectile>(); //get Force value? Not required, as it's automatically set within bullet prefab

        ///TDL, figure out how to auto-assign bullets/etc, without requiring a Resources folder?
        //_bullet = null;//Will require Resources folder, to auto-assign/load; //Resources.Load<GameObject>("Assets/Prefabs/WhiteCell Projectile.prefab");
        ///ref: https://docs.unity3d.com/ScriptReference/Resources.html

        //firepoint transform is automatically set to key rotation, so it shouldn't be an issue.
        //Said manual is set to 2, under "turret_fire" gameobject, transform wise.
    } //end start


    // Update is called once per frame
    private void Update()
    {
        //step A: Adjust aim, to nearest enemy found. Over a 'fluid' amount of time wise
        ///DEPRECATED, due to the lens of "Player should aim turrets", instead of aim itself

        //step B: After X seconds? (Timer wise?), fire bullet. And do an enemy count/check, from those with the "Enemy" Tag/string.
        if (_bps_timer <= 0.0f)
        {
            ///only call TargetEnemy when required upon firing. NOT constantly, to 'save' on RAM/memory usage?
            //TargetEnemy();// foreach inefficient. Could be called at occasionally?
            if (Bullet != null)
            {
                FireCell();
            } //else { Debug.LogWarning("ERROR: No bullet loaded, it's empty."); }

            //endif, last line's a check message
            _bps_timer = 1.0f; //reset timer to "a" second, approximately
        }
        else
        {
            //deduct _bps_timer, by real time/frames.
            _bps_timer -=
                (Time.deltaTime * BulletsPerSecond); //BPS, "accelerates" the ticks, on how fast/often the turret fires.
        } //end if. Under deduct by firing rate, yo

        //Step C: Countdown/deprecate this Game Object, if enabled here past bullet fire code
        Countdown(); //to affect timer, and despawn when time is out
    } //end Start

    //counts down for UI reasons, on top of 'timed/destroy' after a set amount of time, wise
    private void Countdown()
    {
        //that said, IF mindful on pause menu. Consider implementing reference here, to make spawn length 'pause safe'.
        if (_timer > 0.0f)
        {
            _timer -= Time.deltaTime; //to deduct by Time, by frames iteration. Seconds wise.
            _timerGUI.fillAmount =
                (_timer / _setMaxTimer); //dividing by max, always results in 0/1 in 'fill', division wise.
        }
        else
        {
            Destroy(gameObject);
        } //remove from scene.
    }

    #region DeprecatedRotationCode

    ///DEPRECATED,  but still there under the lens of "sample aim bot logic"
    //counts the amount of enemies with the "Enemy" tag (not boss), iteratively setting aim to the nearest enemy to the turret.
    /*    private void TargetEnemy()
        {//ref: https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
            _curTarget = null;//clear the target, before assigning a new object ref
            GameObject[] _foes = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("Amount of Enemies found: " + _foes.Length);

            foreach (GameObject _foe in _foes)
            {
                if (_curTarget == null)
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

    */
    /*  Later excerpt/s, under the lens of 'adjust rotation over time'
             if (_curTarget != null)///Rotation is tilted "2.5D", to bar a 'rotating' turret glitch from clipping the ground
            {//Debug.Log("Adjusting Rotation check...");

                    //first, rotate the tower
                Vector3 _sameAim = _curTarget.position;
                //bar object tilting upwards/downwards, glitch
                _sameAim.y = gameObject.transform.position.y;
                //gameObject.transform.LookAt(_sameAim);//old version, LookAt wise
                Quaternion _slerpAim = Quaternion.LookRotation(
                    _sameAim - gameObject.transform.position
                );//new version, "organic/Slerp" wise. May not be 'as' accurate though by itself
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, _slerpAim, Time.deltaTime
                );//end object transform, turret edition

                    //next, "sharp aim" the barrel, to the 'precise' position of target?
                _firePoint.transform.LookAt(_curTarget);//haaack 'tinker'
                //end aim adjustment, 'zone in' wise.

                ///PS: Object aimed at would have been "_curTarget", IF the 'barrel/head' can rotate without issue
            }//end "if there's a target" if


     */

    #endregion

    //fire a projectile, from the "White Cell tower", defense bullet wise.
    private void FireCell()
    {
        TurretInvoke();
        AkSoundEngine.PostEvent("Play_White_Cell_Tower_Shoot", gameObject);
        _animator.SetTrigger("Shoot");


        //how to aim = "Raycast to Boss, to alter/change rotation, yo" :)

        //no need to adjust rotation, as that's all~ set up in the "_firePoint" transform
        GameObject go = Instantiate(Bullet,
            _firePoint.transform.position,
            _firePoint.transform.rotation);
        //use in-script velocity, over adding velocity directly, here
        go.GetComponent<Projectile>().Shoot(_firePoint.transform.forward,
            (BulletVelocity * 1000));
//        Rigidbody rb = go.GetComponent<Rigidbody>();
//        rb.AddForce(transform.forward * (BulletVelocity*10)); //projectile script does it in-object
        //alternate velocity, before mindfulness kicked in
//        Debug.LogWarning("Object is spawned at: " + go.transform.position + ", Time Spawned = " + Time.fixedTime);
    }

    private void TurretInvoke()
    {
        _turretSpark.SetActive(!_turretSpark.activeSelf);
    }

    public void DestroyTower()
    {
        AkSoundEngine.PostEvent("Play_White_Cell_Tower_Destoyed", gameObject);
    }
}