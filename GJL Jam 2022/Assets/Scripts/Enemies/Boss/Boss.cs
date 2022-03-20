using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    [SerializeField] private float _throwDelay = 5f;

    [SerializeField, Tooltip("The height above spawn at which the thrown minion which reach its peak")]
    private float _throwHeight = 15f;

    [Header("Stoic Attributes")] [SerializeField]
    private float _stoicDelay = 10f;

    [SerializeField] private float _stoicDuration = 3f;

    public UnityEvent OnMinionThrown;

    private Animator _animator;
    private float _currentStoicTime;
    private float _currentMinionThrowTime;

    public bool IsInStoicState { get; set; }

    private void Start() => _animator = GetComponentInChildren<Animator>(true);
    private void OnEnable() => ThrowMinionAnimation.OnBossThrow += ThrowMinion;

    private void OnDisable() => ThrowMinionAnimation.OnBossThrow -= ThrowMinion;

    /* Called by Animation event */
    [ContextMenu("Throw Minion")]
    public void ThrowMinion()
    {
        GameObject minion = _minionSpawner.SpawnStickyMinion(_minionSpawnPoint);
        minion.GetComponent<StickyMinion>().JumpAtPlayer(_throwHeight);
        OnMinionThrown.Invoke();
    }

    private void Update()
    {
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

    private IEnumerator ToggleStoicState()
    {
        IsInStoicState = true;
        yield return new WaitForSeconds(_stoicDuration);
        IsInStoicState = false;
    }
}