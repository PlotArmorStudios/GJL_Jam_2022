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

public enum MinionState { Spawning, ChasePlayer, StuckToPlayer, Dead }

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth), typeof(Rigidbody))]
public class StickyMinion : MonoBehaviour
{   
    [Header("Movement Attributes")]
    [SerializeField, Tooltip("How long in seconds the minion waits after spawning before chasing the player")] private float _timeBeforeMove = 0.5f;
    [SerializeField, Tooltip("How close the minion has to get to the player in order to stick")] private float _distanceWhenStick = 3f;

    [Header("Damage Attributes")]
    [SerializeField] private float _timeBetweenAttacks = 1f;
    [SerializeField, Tooltip("Time in seconds before player starts taking damage when enemy is stuck")] private float _timeBeforePlayerDamage;
    [SerializeField, Tooltip("How many times a stuck enemy will damage the player before it dies")] private int _numAttacks = 3;
    
    [Header("Events")]
    public UnityEvent OnSpawn;
    public UnityEvent OnStartChase;
    public UnityEvent OnStickToPlayer;
    public UnityEvent OnHitPlayer;
    public UnityEvent OnDie;

    //General Minion Attributes
    public MinionStats Stats { get; set; }
    private MinionState _state;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidBody;
    private EnemyHealth _health;
    private Collider[] _colliders;
    private Transform _parent;
    
    //Player Variables
    private Transform _player;
    private BodyPartManager _playerBodyParts;
    private int _bodyPartsLayerMask = 1 << 8;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _playerBodyParts = _player.GetComponent<BodyPartManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
        _health = GetComponent<EnemyHealth>();
        _health.OnDie += Die;
        _colliders = GetComponents<Collider>();
        PlayerHealth.OnDamageKidney += UnstickFromPlayer;
        _parent = transform.parent;
    }

    private void OnEnable() 
    {
        ChangeState(MinionState.Spawning);
        _health.ResetHealth();
        _navMeshAgent.enabled = false;
        _rigidBody.isKinematic = true;
        SetCollidersActive(true);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag != "Player" || _state != MinionState.ChasePlayer) return;
        
        ChangeState(MinionState.StuckToPlayer);
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
        _rigidBody.isKinematic = true;

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
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        ChangeState(MinionState.Dead);
    }

    private void StickToPlayer()
    {
        OnStickToPlayer?.Invoke();

        _navMeshAgent.enabled = false;
        
        Transform bodyPart = _playerBodyParts.GetRandomBodyPart();
        
        
        Vector3 targetPosition = bodyPart.TransformPoint(bodyPart.GetComponent<BoxCollider>().center);
        RaycastHit hit;
        Physics.Raycast(transform.position, targetPosition - transform.position, out hit, 30, _bodyPartsLayerMask);
        Debug.DrawLine(transform.position, targetPosition, Color.blue, 2);
        transform.localPosition = hit.point + (transform.localPosition - targetPosition).normalized * (GetComponent<MeshRenderer>().bounds.size.x / 2);
        Debug.Log(GetComponent<SphereCollider>().name + " collider has a size of " + ((transform.localPosition - targetPosition).normalized * (GetComponent<MeshRenderer>().bounds.size.x / 2)));
        transform.SetParent(hit.collider.gameObject.transform);;
        SetCollidersActive(false);
    }

    public void UnstickFromPlayer(float unused)
    {
        _rigidBody.isKinematic = false;
        transform.SetParent(_parent);
    }

    private IEnumerator TriggerDeath()
    {
        OnDie?.Invoke();

        UnstickFromPlayer(0f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void SetCollidersActive(bool active)
    {
        foreach (Collider c in _colliders)
        {
            c.enabled = active;
        }
    }

    private void ChangeState(MinionState state)
    {
        _state = state;
        switch(state) 
        {
            case MinionState.Spawning:
                StartCoroutine(Spawn());
                break;
            case MinionState.ChasePlayer:
                StartCoroutine(FollowPlayer());
                break;
            case MinionState.StuckToPlayer:
                StickToPlayer();
                StartCoroutine(DealDamageOverTime());
                break;
            case MinionState.Dead:
                StartCoroutine(TriggerDeath());
                break;
        }
    }

    private void Die()
    {
        ChangeState(MinionState.Dead);
    }
}