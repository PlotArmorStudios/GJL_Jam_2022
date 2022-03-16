using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum MinionState { Spawning, ChasePlayer, StuckToPlayer, Dead }

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
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
    public UnityEvent OnDie;

    public MinionStats Stats { get; set; }
    private MinionState _state;

    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    private EnemyHealth _health;
    private Collider[] _colliders;
    
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<EnemyHealth>();
        _health.OnDie += Die;
        _colliders = GetComponents<Collider>();
    }

    private void OnEnable() 
    {
        ChangeState(MinionState.Spawning);
        _health.ResetHealth();
        _navMeshAgent.enabled = false;
        SetCollidersActive(true);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag != "Player" || _state != MinionState.ChasePlayer) return;
        
        Debug.Log("GOT EEM");
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
        while (_state == MinionState.ChasePlayer)
        {
            Debug.Log("Following player");
            _navMeshAgent.destination = _player.position;
            yield return new WaitForSeconds(0.1f);
        }
        _navMeshAgent.enabled = false;
    }
    
    private IEnumerator DealDamageOverTime()
    {
        yield return new WaitForSeconds(_timeBeforePlayerDamage);

        for (int i = 0; i < _numAttacks; i++)
        {
            _player.GetComponent<Health>().TakeDamage(Stats._damage);
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        ChangeState(MinionState.Dead);
    }

    private IEnumerator StickToPlayer()
    {
        OnStickToPlayer?.Invoke();

        SetCollidersActive(false);
        Vector3 offset = transform.position - _player.position;
        while (_state == MinionState.StuckToPlayer)
        {
            transform.position = _player.position + offset;
            yield return null;
        }
    }

    private IEnumerator TriggerDeath()
    {
        OnDie?.Invoke();

        yield return new WaitForSeconds(1f);
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
        Debug.Log("Switched state to " + state);
        switch(state) 
        {
            case MinionState.Spawning:
                StartCoroutine(Spawn());
                break;
            case MinionState.ChasePlayer:
                StartCoroutine(FollowPlayer());
                break;
            case MinionState.StuckToPlayer:
                StartCoroutine(DealDamageOverTime());
                StartCoroutine(StickToPlayer());
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
