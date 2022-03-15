using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StickyMinionPool))]
public class MinionSpawner : MonoBehaviour
{
    
    [SerializeField, Tooltip("Minions can spawn from any transform in this array")] private Transform[] _spawnPoints;
    [Header("Minion attribute bounds")]
    [SerializeField] private int _maxLevel;
    private int _currMaxLevel, _currMinLevel;
    [SerializeField] private float _minSize, _maxSize;
    [SerializeField] private float _minDamage, _maxDamage;
    [SerializeField] private float _minSpeed, _maxSpeed;

    private StickyMinionPool _stickyMinionPool;

    private void Awake() 
    {
        _stickyMinionPool =  GetComponent<StickyMinionPool>();
    }

    private void OnEnable() 
    {
        _currMaxLevel = 1;
        _currMinLevel = 0;
    }

    public void IncreaseMaxLevel()
    {
        _currMaxLevel++;
        if (_currMaxLevel >= 5)
        {
            _currMinLevel++;
        }
    }

    public void SpawnStickyMinion()
    {
        int level = Random.Range(_currMinLevel, _currMaxLevel);
        float multiplier = level / _maxLevel;
        MinionStats stats = new MinionStats(
            level,
            _minDamage + (_maxDamage * multiplier),
            _minSpeed + (_maxSpeed * multiplier),
            _minSize + (_maxSize * multiplier)
        );
        GameObject minion = _stickyMinionPool.GetObject(stats);
        minion.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)].position;
    }
}
