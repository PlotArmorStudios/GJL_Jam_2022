using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum BossState
{
    ChargingThrow,
    Throwing,
    Waiting,
    TakeDamage,
    Stopped
}

public class Boss : MonoBehaviour
{
    public static event Action<float> OnBossDamage;

    [Header("Kidney Attack Attributes")] [SerializeField]
    private float _attackPower;

    [SerializeField] private float _attackDelay;

    private float _currentAttackTime;

    [Header("Minion Spawning Attributes")] [SerializeField]
    private Transform _minionSpawnPoint;

    [SerializeField] private MinionSpawner _minionSpawner;

    [SerializeField, Tooltip("The height above spawn at which the thrown minion which reach its peak")]
    private float _throwHeight = 10f;
    [SerializeField] private float _timeBetweenThrows = 5f;
    [SerializeField, Tooltip("How long the boss will wait to start throwing minions when the game starts or the player dies")] float _timeUntilThrowMinions = 10f;
    [SerializeField, Tooltip("How long the boss is unable to throw minions for after taking damage")] private float _delayWhenTakeDamage = 3f;
    private IEnumerator _waitingRoutine, _throwingRoutine;
    [SerializeField] private float _throwDelay = 4f;

    [Header("Stoic Attributes")] [SerializeField]
    private float _stoicDelay = 10f;

    [SerializeField] private float _stoicDuration = 3f;

    public UnityEvent OnMinionThrown;
    public UnityEvent OnFinishThrowing;

    private Animator _animator;
    private float _currentStoicTime;
    private float _currentMinionThrowTime;
    private BossState _state;

    public bool IsInStoicState { get; set; }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>(true);
        ChangeState(BossState.Waiting);
        BossHealth.OnTakeDamage += TakeDamage;
        PlayerHealth.OnPlayerDeath += StartWait;
    }
    private void OnEnable() => ThrowMinionAnimation.OnBossThrow += ThrowMinion;

    private void OnDisable() => ThrowMinionAnimation.OnBossThrow -= ThrowMinion;

    /* Called by Animation event */
    [ContextMenu("Throw Minion")]
    public void ThrowMinion()
    {
        GameObject minion = _minionSpawner.SpawnStickyMinion(_minionSpawnPoint);
        minion.GetComponent<StickyMinion>().JumpAtPlayer(_throwHeight);
        OnMinionThrown.Invoke();
        
        //delete this once Finishthrow animation event linked
        FinishThrow();
    }

    /* Called by Animation event */
    public void FinishThrow()
    {
        ChangeState(BossState.ChargingThrow);
        OnFinishThrowing?.Invoke();
    }

    [ContextMenu("Stop Throwing Minions")]
    public void StopThrowingMinions()
    {
        StopAllCoroutines();
        ChangeState(BossState.Stopped);
    }

    private void Update()
    {
        if (_state == BossState.Stopped) return;

        _currentAttackTime += Time.deltaTime;

        if (_currentAttackTime >= _attackDelay)
        {
            OnBossDamage?.Invoke(_attackPower);
            _currentAttackTime = 0;
        }

        _currentMinionThrowTime += Time.deltaTime;
        
        if (_currentMinionThrowTime >= _throwDelay)
        {
            _animator.SetTrigger("Throw");
        }

        _currentStoicTime += Time.deltaTime;

        if (_currentStoicTime >= _stoicDelay)
        {
            StartCoroutine(ToggleStoicState());
            _currentStoicTime = 0;
        }
    }

    private void ChangeState(BossState state)
    {
        if (_throwingRoutine != null) StopCoroutine(_throwingRoutine);
        if (_waitingRoutine != null) StopCoroutine(_waitingRoutine);
        switch (state)
        {
            case BossState.ChargingThrow:
                _throwingRoutine = ChargeThrow();
                StartCoroutine(_throwingRoutine);
                break;
            case BossState.Throwing:
                //_animator.SetTrigger("Throw");
                
                //delete this once animation linked
                ThrowMinion();
                break;
            case BossState.Waiting:
                _waitingRoutine = WaitForPlayer(_timeUntilThrowMinions); 
                StartCoroutine(_waitingRoutine);
                Debug.Log("Waiting");
                break;
            case BossState.TakeDamage:
                _waitingRoutine = WaitForPlayer(_delayWhenTakeDamage); 
                StartCoroutine(_waitingRoutine);
                Debug.Log("Damaged");
                break;
            case BossState.Stopped:
                break;
        }
    }

    public BossState GetBossState()
    {
        return _state;
    }

    [ContextMenu("Boss takes damage")]
    private void TakeDamageTest()
    {
        TakeDamage(10);
    }

    public void TakeDamage(float healthPercent)
    {
        while (1 - healthPercent > _minionSpawner.GetLevelProgress())
        {
            _minionSpawner.IncreaseMaxLevel();
        }
        if (!IsInStoicState)
        {
            ChangeState(BossState.TakeDamage);
        }
    }

    private IEnumerator ChargeThrow()
    {
        yield return new WaitForSeconds(_timeBetweenThrows);
        ChangeState(BossState.Throwing);
    }

    [ContextMenu("WaitForPlayer")]
    private void WaitForPlayerTest()
    {
        StartWait(0);
    }

    public void StartWait(float unused)
    {
        ChangeState(BossState.Waiting);
    }

    private IEnumerator WaitForPlayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangeState(BossState.Throwing);
    }

    private IEnumerator ToggleStoicState()
    {
        IsInStoicState = true;
        yield return new WaitForSeconds(_stoicDuration);
        IsInStoicState = false;
    }
}