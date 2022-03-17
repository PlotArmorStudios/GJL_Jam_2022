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
    [SerializeField] float _minTimeBetweenSpawns = 1f, _maxTimeBetweenSpawns = 5f;
    float _currTimeBetweenSpawns;

    [Header("Minion attribute bounds")]
    [SerializeField] private int _maxLevel = 10;
    private int _currMaxLevel, _currMinLevel;
    /* Min values are for minions at level 0, max values are for minions at max level */
    [SerializeField] private float _minSize = 0.2f, _maxSize = 0.7f;
    [SerializeField] private float _minDamage = 0.5f, _maxDamage = 10f;
    [SerializeField] private float _minSpeed = 2f, _maxSpeed = 10f;
    [SerializeField] private int _minStartingHealth = 10, _maxStartingHealth = 100;
    private StickyMinionPool _stickyMinionPool;
    SpawnerState _state = SpawnerState.Spawning;
    private Transform[] _spawnPoints;


    public UnityEvent OnStartSpawning;
    public UnityEvent OnPauseSpawning;

    private void Awake() 
    {
        _stickyMinionPool =  GetComponent<StickyMinionPool>();
        InitaliseSpawnPoints();
        PlayerHealth.OnPlayerDeath += StopSpawning;
    }

    private void InitaliseSpawnPoints()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();
        Transform[] spawns = new Transform[_spawnPoints.Length - 1];
        int count = 0;
        foreach (Transform spawn in _spawnPoints)
        {
            if (spawn.gameObject != gameObject)
            {
                spawns[count] = spawn;
                count++;
            }
        }
        _spawnPoints = spawns;
    }

    private void OnEnable() 
    {
        _currMaxLevel = 4;
        _currMinLevel = 0;
        _currTimeBetweenSpawns = _minTimeBetweenSpawns;
    }

    private void Start() 
    {
        ChangeState(SpawnerState.Spawning);
    }

    public void IncreaseMaxLevel()
    {
        _currMaxLevel++;
        if (_currMaxLevel >= _randomLevelVariation)
        {
            _currMinLevel++;
        }
    }

    public void StartSpawning(float timeBeforeStartSpawning)
    {
        StartCoroutine(SpawnEnemies(_currTimeBetweenSpawns, timeBeforeStartSpawning));
    }

    private void StopSpawning(float unused = 0f)
    {
        Debug.Log("Player respawned");
        StopAllCoroutines();
        StartSpawning(10f);
    }

    private IEnumerator SpawnEnemies(float timeBetweenSpawns, float timeBeforeStart)
    {
        yield return new WaitForSeconds(timeBeforeStart);
        while (_state == SpawnerState.Spawning)
        {
            SpawnStickyMinion();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void ChangeState(SpawnerState state)
    {
        _state = state;

        switch (_state)
        {
            case SpawnerState.Spawning:
                StartSpawning(0f);
                OnStartSpawning?.Invoke();
                break;
            case SpawnerState.Waiting:
                OnPauseSpawning?.Invoke();
                break;
        }
    }

    public void SpawnStickyMinion()
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
        minion.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        minion.SetActive(true);
    }
}
