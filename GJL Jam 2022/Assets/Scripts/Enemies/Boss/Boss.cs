using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Boss : MonoBehaviour
{
    public static event Action<float> OnBossDamage;
    
    [Header("Kidney Attack Attributes")]
    [SerializeField] private float _attackPower;
    [SerializeField] private float _attackDelay;

    private float _currentAttackTime;

    [Header("Minion Spawning Attributes")]
    [SerializeField] private Transform _minionSpawnPoint;
    [SerializeField] private MinionSpawner _minionSpawner;
    [SerializeField, Tooltip("The height above spawn at which the thrown minion which reach its peak")] private float _throwHeight = 15f;

    public UnityEvent OnMinionThrown;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>(true);
    }

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
    }
}
