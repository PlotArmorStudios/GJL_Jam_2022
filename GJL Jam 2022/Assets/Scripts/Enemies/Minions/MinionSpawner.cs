/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SpawnerState { Spawning, Waiting }

[RequireComponent(typeof(StickyMinionPool))]
public class MinionSpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    [SerializeField, Tooltip("How much lower than current max level enemies can be")] private int _randomLevelVariation = 5; 

    [Header("Minion attribute bounds")]
    [SerializeField] private int _maxLevel = 10;
    private int _currMaxLevel, _currMinLevel;
    /* Min values are for minions at level 0, max values are for minions at max level */
    [SerializeField] private float _minSize = 0.2f, _maxSize = 0.7f;
    [SerializeField] private float _minDamage = 0.5f, _maxDamage = 10f;
    [SerializeField] private float _minSpeed = 2f, _maxSpeed = 10f;
    [SerializeField] private int _minStartingHealth = 10, _maxStartingHealth = 100;
    private StickyMinionPool _stickyMinionPool;    

    public UnityEvent OnStartSpawning;
    public UnityEvent OnPauseSpawning;

    private void Awake() 
    {
        _stickyMinionPool =  GetComponent<StickyMinionPool>();
    }

    private void OnEnable() 
    {
        _currMaxLevel = 4;
        _currMinLevel = 0;
    }

    public void IncreaseMaxLevel()
    {
        _currMaxLevel++;
        if (_currMaxLevel >= _randomLevelVariation)
        {
            _currMinLevel++;
        }
    }

    public GameObject SpawnStickyMinion(Transform spawnPoint)
    {
        
        int level = Random.Range(_currMinLevel, _currMaxLevel + 1);
        float multiplier = (float)level / _maxLevel;
        MinionStats stats = new MinionStats(
            level,
            _minDamage + (_maxDamage - _minDamage) * multiplier,
            _maxSpeed - (_maxSpeed - _minSpeed) * multiplier,
            _minSize + (_maxSize - _minSize) * multiplier
        );
        int maxHealth = (int)(_minStartingHealth + _maxStartingHealth * multiplier);
        GameObject minion = _stickyMinionPool.GetObject(stats, maxHealth);
        minion.transform.position = spawnPoint.position;
        minion.SetActive(true);
        return minion;
    }
}