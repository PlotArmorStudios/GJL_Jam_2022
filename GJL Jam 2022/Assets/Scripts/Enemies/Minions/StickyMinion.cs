/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum MinionState { Spawning, ChasePlayer, Sticking, StuckToPlayer, Dead }

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
public class StickyMinion : MonoBehaviour
{   
    [Header("Movement Attributes")]
    [SerializeField, Tooltip("How long in seconds the minion waits after spawning before chasing the player")] private float _timeBeforeMove = 0.5f;

    [Header("Damage Attributes")]
    [SerializeField] private float _timeBetweenAttacks = 1f;
    [SerializeField, Tooltip("Time in seconds before player starts taking damage when enemy is stuck")] private float _timeBeforePlayerDamage;
    [SerializeField, Tooltip("How many times a stuck enemy will damage the player before it dies")] private int _numAttacks = 3;
    
    //General Attributes
    public MinionStats Stats { get; set; }
    private MinionState _state;
    private NavMeshAgent _navMeshAgent;
    private EnemyHealth _health;
    private CapsuleCollider _triggerZone;
    private SphereCollider _collider;
    private Rigidbody _rigidbody;
    private Transform _parent;
    
    //Player Variables
    private Transform _player;
    private BodyPartManager _playerBodyParts;
    private int _bodyPartsLayerMask = 1 << 8;
    
    [Header("Events")]
    public UnityEvent OnSpawn;
    public UnityEvent OnStartChase;
    public UnityEvent OnJumpAtPlayer;
    public UnityEvent OnStickToPlayer;
    public UnityEvent OnHitPlayer;
    public UnityEvent OnDie;

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
        PlayerHealth.OnDamageKidney += Die;
    }

    private void OnEnable() 
    {
        ChangeState(MinionState.Spawning);
        _health.ResetHealth();
        _navMeshAgent.enabled = false;
        _triggerZone.enabled = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = true;
        gameObject.layer = 0;
        _parent = transform.parent;
        _navMeshAgent.speed = Stats._speed;
    }

    private void Start() 
    {
        _navMeshAgent.speed = Stats._speed;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && _state == MinionState.ChasePlayer)
            ChangeState(MinionState.Sticking);
    }

    private IEnumerator Spawn()
    {
        OnSpawn?.Invoke();

        yield return new WaitForSeconds(_timeBeforeMove);
        ChangeState(MinionState.ChasePlayer);
    }
    
    private IEnumerator FollowPlayer()
    {
        OnStartChase?.Invoke();

        _navMeshAgent.enabled = true;

        while (_state == MinionState.ChasePlayer)
        {
            _navMeshAgent.destination = _player.position;
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
            Debug.Log("hit player");
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
        
        transform.SetParent(_player);

        Transform bodyPart = _playerBodyParts.GetRandomBodyPart();
        Vector3 bodyPartRayTarget = bodyPart.TransformPoint(bodyPart.GetComponent<BoxCollider>().center);
        Vector3 startPosition = transform.position;
        RaycastHit hit;
        Physics.Raycast(startPosition, bodyPartRayTarget - startPosition, out hit, 30, _bodyPartsLayerMask);
        Transform targetPoint = new GameObject().transform;
        targetPoint.position = hit.point + (transform.position - hit.point).normalized * (GetComponent<MeshRenderer>().bounds.size.x / 2);
        targetPoint.SetParent(bodyPart);
        

        float distanceToPlayer = Vector3.Distance(startPosition, targetPoint.position);
        Vector3 velocity = Vector3.zero;
        while (distanceToPlayer > 0.1f)
        {
            Debug.Log("Jumping");
            transform.position = Vector3.SmoothDamp(transform.position, targetPoint.position, ref velocity, 0.1f);
            distanceToPlayer = Vector3.Distance(transform.position, targetPoint.position);
            yield return null;
        }
        transform.SetParent(hit.collider.gameObject.transform);
        ChangeState(MinionState.StuckToPlayer);
        OnStickToPlayer?.Invoke();
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

    private void ChangeState(MinionState state)
    {
        if (gameObject.activeSelf == false) return;
        _state = state;
        switch(state) 
        {
            case MinionState.Spawning:
                Debug.Log("Spawning");
                StartCoroutine(Spawn());
                break;
            case MinionState.ChasePlayer:
                Debug.Log("Chasing");
                StartCoroutine(FollowPlayer());
                break;
            case MinionState.Sticking:
                Debug.Log("Sticking");
                StartCoroutine(StickToPlayer());
                break;
            case MinionState.StuckToPlayer:
                Debug.Log("Stuck");
                StartCoroutine(DealDamageOverTime());
                break;
            case MinionState.Dead:
                Debug.Log("Dead");
                StartCoroutine(TriggerDeath());
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
}