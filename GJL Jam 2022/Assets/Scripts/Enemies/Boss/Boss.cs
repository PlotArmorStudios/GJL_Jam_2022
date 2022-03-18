using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Boss : MonoBehaviour
{
    public static event Action<float> OnBossDamage;

    [SerializeField] private float _attackPower;
    [SerializeField] private float _attackDelay;

    private float _currentAttackTime;

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