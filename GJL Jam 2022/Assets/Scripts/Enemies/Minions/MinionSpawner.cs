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
    /* Min values are for minions at level 0, max values are for minions at max level */
    [SerializeField] private float _minSize = 0.5f, _maxSize = 1.5f;
    [SerializeField] private float _minDamage = 0.5f, _maxDamage = 10f;
    [SerializeField] private float _minSpeed = 1f, _maxSpeed = 10f;
    [SerializeField] private int _minStartingHealth = 10, _maxStartingHealth = 100;

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
            _minDamage + _maxDamage * multiplier,
            _maxSpeed - (_maxSpeed - _minSpeed) * multiplier,
            _minSize + _maxSize * multiplier
        );
        int maxHealth = (int)(_minStartingHealth + _maxStartingHealth * multiplier);
        GameObject minion = _stickyMinionPool.GetObject(stats, maxHealth);
        minion.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)].position;
    }
}
