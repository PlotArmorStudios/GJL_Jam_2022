using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum MinionState { Spawning, ChasePlayer, StuckToPlayer, Dead }

[RequireComponent(typeof(NavMeshAgent))]
public class StickyMinion : Enemy
{   
    [Header("Movement Attributes")]
    [SerializeField, Tooltip("How long in seconds the minion waits after spawning before chasing the player")] private float _timeBeforeMove = 0.5f;
    [SerializeField, Tooltip("How  close the minion has to get to the player in order to stick")] private float _distanceWhenStick = 3f;

    [Header("Damage Attributes")]
    [SerializeField] private float _timeBetweenAttacks = 1f;
    [SerializeField, Tooltip("Time in seconds before player starts taking damage when enemy is stuck")] private float _timeBeforePlayerDamage;
    [SerializeField, Tooltip("How many times a stuck enemy will damage the player before it dies")] private int _numAttacks = 3;
    
    [Header("Events")]
    public UnityEvent _onSpawn;
    public UnityEvent _onStartChase;
    public UnityEvent _onStickToPlayer;
    public UnityEvent _onDie;

    public MinionStats Stats { get; set; }
    private MinionState _state;

    private Transform _player;
    private NavMeshAgent _navMeshAgent;
    
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable() 
    {
        ChangeState(MinionState.Spawning);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag != "Player") return;
        
        Debug.Log("GOT EEM");
        ChangeState(MinionState.StuckToPlayer);
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_timeBeforeMove);
        ChangeState(MinionState.ChasePlayer);
    }
    
    private IEnumerator FollowPlayer()
    {
        while (_state == MinionState.ChasePlayer)
        {
            Debug.Log("Following player");
            _navMeshAgent.destination = _player.position;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
    private IEnumerator DamageOverTime()
    {
        yield return new WaitForSeconds(_timeBeforePlayerDamage);

        for (int i = 0; i < _numAttacks; i++)
        {
            //_player.GetComponent<Health>().TakeDamage()
        }
    }

    private void ChangeState(MinionState state)
    {
        _state = state;

        switch(state) 
        {
            case MinionState.Spawning:
                _onSpawn.Invoke();
                Spawn();
                break;
            case MinionState.ChasePlayer:
                _onStartChase.Invoke();
                StartCoroutine(FollowPlayer());
                break;
            case MinionState.StuckToPlayer:
                _onStickToPlayer.Invoke();
                StartCoroutine(DamageOverTime());
                break;
            case MinionState.Dead:
                _onDie.Invoke();
                break;
        }
    }

    protected override void Die()
    {
        ChangeState(MinionState.Dead);
    }
}
