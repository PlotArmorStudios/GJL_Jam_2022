/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/

//#define DebugStateMachine
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum MinionState
{
    Spawning,
    ChasePlayer,
    Sticking,
    StuckToPlayer,
    Dead, 
    Freeze
}

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
public class StickyMinion : MonoBehaviour
{
    const float GRAVITY = -9.81f;

    [Header("Movement Attributes")]
    [SerializeField, Tooltip("How long in seconds the minion waits after spawning before chasing the player")]
    private float _timeBeforeMove = 0.5f;

    [Header("Damage Attributes")] [SerializeField]
    private float _timeBetweenAttacks = 1f;

    [SerializeField, Tooltip("Time in seconds before player starts taking damage when enemy is stuck")]
    private float _timeBeforePlayerDamage;

    [SerializeField, Tooltip("How many times a stuck enemy will damage the player before it dies")]
    private int _numAttacks = 3;

    //General Attributes
    public MinionStats Stats { get; set; }
    private MinionState _state;
    private NavMeshAgent _navMeshAgent;
    private EnemyHealth _health;
    private CapsuleCollider _triggerZone;
    private SphereCollider _collider;
    private Rigidbody _rigidbody;
    private Transform _parent;
    private Animator _animator;

    //Player Variables
    private Transform _player;
    private BodyPartManager _playerBodyParts;
    private int _bodyPartsLayerMask = 1 << 8;
    private bool _inRangeOfPlayer;

    [Header("Events")] public UnityEvent OnSpawn;
    public UnityEvent OnStartChase;
    public UnityEvent OnJumpAtPlayer;
    public UnityEvent OnStickToPlayer;
    public UnityEvent OnHitPlayer;
    public UnityEvent OnDie;
    private AddToAmmo _addToAmmo;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _playerBodyParts = _player.GetComponent<BodyPartManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<EnemyHealth>();
        _health.OnDie += Die;
        _triggerZone = GetComponent<CapsuleCollider>();
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _addToAmmo = GetComponent<AddToAmmo>();
        _animator = GetComponentInChildren<Animator>();
        PlayerHealth.OnPlayerDeath += Die;
    }

    private void OnEnable()
    {
        ChangeState(MinionState.Spawning);
        _health.ResetHealth();
        _navMeshAgent.enabled = false;
        _triggerZone.enabled = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        gameObject.layer = 0;
        _parent = transform.parent;
        _navMeshAgent.speed = Stats._speed;
        _inRangeOfPlayer = false;
    }

    private void Start()
    {
        _navMeshAgent.speed = Stats._speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { 
            if (_state == MinionState.ChasePlayer)
            {
                ChangeState(MinionState.Sticking);
            }
            else
            {
                _inRangeOfPlayer = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && _state == MinionState.Spawning)
        {
            _rigidbody.isKinematic = true;
            _navMeshAgent.enabled = false;
            _collider.enabled = false;
            _triggerZone.enabled = false;
            transform.SetParent(_player);
            
            ChangeState(MinionState.StuckToPlayer);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && _state == MinionState.Spawning)
        {
            _rigidbody.isKinematic = true;
            _animator.SetTrigger("StartRunning");
            ChangeState(MinionState.ChasePlayer);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag == "Player")
        {
            _inRangeOfPlayer = false;
        }    
    }

    private void Spawn()
    {
        OnSpawn?.Invoke();
    }

    public void JumpAtPlayer(float throwHeight)
    {
        _rigidbody.isKinematic = false; 
        _navMeshAgent.enabled = false;
        
        float displacementY = _player.position.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(_player.position.x - transform.position.x, 0, _player.position.z - transform.position.z);
        
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * GRAVITY * throwHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * throwHeight/GRAVITY) + Mathf.Sqrt(2*(displacementY - throwHeight) / GRAVITY));

        _rigidbody.velocity = velocityY + velocityXZ;
    }

    private IEnumerator FollowPlayer()
    {
        OnStartChase?.Invoke();

        _navMeshAgent.enabled = true;

        while (_state == MinionState.ChasePlayer)
        {
            _navMeshAgent.destination = _player.position;
            Debug.Log("Set navmesh destination to " + _navMeshAgent.destination);
            Debug.Log("Current position: " + transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DealDamageOverTime()
    {
        yield return new WaitForSeconds(_timeBeforePlayerDamage);

        for (int i = 0; i < _numAttacks; i++)
        {
            _player.GetComponent<Health>().TakeDamage(Stats._damage);
            OnHitPlayer.Invoke();
#if DebugStateMachine
            Debug.Log("hit player");
#endif
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        ChangeState(MinionState.Dead);
    }

    private IEnumerator StickToPlayer()
    {
        OnJumpAtPlayer?.Invoke();

        _navMeshAgent.enabled = false;
        _collider.enabled = false;
        _triggerZone.enabled = false;

        _animator.SetTrigger("JumpStick");

        transform.SetParent(_player);

        Transform bodyPart = _playerBodyParts.GetRandomBodyPart();
        Vector3 bodyPartRayTarget = bodyPart.TransformPoint(bodyPart.GetComponent<BoxCollider>().center);
        Vector3 startPosition = transform.position;
        RaycastHit hit;
        Physics.Raycast(startPosition, bodyPartRayTarget - startPosition, out hit, 30, _bodyPartsLayerMask);
        Transform targetPoint = new GameObject().transform;
        targetPoint.position = hit.point + (transform.position - hit.point).normalized *
            (GetComponent<MeshRenderer>().bounds.size.x / 2);
        targetPoint.SetParent(bodyPart);


        float distanceToPlayer = Vector3.Distance(startPosition, targetPoint.position);
        Vector3 velocity = Vector3.zero;
        while (distanceToPlayer > 0.1f)
        {
#if DebugStateMachine
            Debug.Log("Jumping");
#endif
            transform.position = Vector3.SmoothDamp(transform.position, targetPoint.position, ref velocity, 0.1f);
            distanceToPlayer = Vector3.Distance(transform.position, targetPoint.position);
            yield return null;
        }

        transform.SetParent(hit.collider.gameObject.transform);
        ChangeState(MinionState.StuckToPlayer);
    }

    public void UnstickFromPlayer(float unused)
    {
        transform.SetParent(_parent);
        StopCoroutine(DealDamageOverTime());
        _rigidbody.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        _collider.enabled = true;
    }

    private IEnumerator TriggerDeath()
    {
        OnDie?.Invoke();

        UnstickFromPlayer(0f);
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator Frozen()
    {
        _navMeshAgent.enabled = false;
        _triggerZone.enabled = false;
        float prevSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(3f);
        _navMeshAgent.enabled = true;
        _triggerZone.enabled = true;
        _animator.speed = prevSpeed;
    }

    private void ChangeState(MinionState state)
    {
        if (gameObject.activeSelf == false) return;
        _state = state;
        switch (state)
        {
            case MinionState.Spawning:
#if DebugStateMachine
                Debug.Log("Spawning");
#endif
                Spawn();
                break;
            case MinionState.ChasePlayer:
#if DebugStateMachine
                Debug.Log("Chasing");
#endif
                if (_inRangeOfPlayer)
                {
                    ChangeState(MinionState.Sticking);
                }
                else
                {
                    StartCoroutine(FollowPlayer());
                }
                break;
            case MinionState.Sticking:
#if DebugStateMachine
                Debug.Log("Sticking");
#endif
                StartCoroutine(StickToPlayer());
                break;
            case MinionState.StuckToPlayer:
#if DebugStateMachine
                Debug.Log("Stuck");
#endif          
                OnStickToPlayer.Invoke();
                StartCoroutine(DealDamageOverTime());
                break;
            case MinionState.Dead:
#if DebugStateMachine
                Debug.Log("Dead");
#endif
                StartCoroutine(TriggerDeath());
                break;
            case MinionState.Freeze:
                StartCoroutine(Frozen());
                break;
        }
    }

    private void Die()
    {
        transform.SetParent(_parent);
        ChangeState(MinionState.Dead);
    }

    private void Die(float unused)
    {
        transform.SetParent(_parent);
        if (_state == MinionState.Sticking || _state == MinionState.StuckToPlayer)
            ChangeState(MinionState.Dead);
    }

    public void Freeze()
    {
        _addToAmmo.OnFreezeMinion();
        ChangeState(MinionState.Freeze);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= Die;
    }
}