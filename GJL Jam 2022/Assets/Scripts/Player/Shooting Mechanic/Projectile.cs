using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] private float _targetingDelay = .5f;

    protected StickyMinion[] _enemiesInScene;
    protected Transform _closestEnemy;

    private Rigidbody _rigidbody;
    private bool _target;

    //Can be used for specific aim direction
    public Vector3 Direction { get; set; }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _closestEnemy = GetClosestEnemy();
        _target = false;
        StartCoroutine(DelayEnemyTargeting());
    }

    private IEnumerator DelayEnemyTargeting()
    {
        yield return new WaitForSeconds(_targetingDelay);
        _target = true;
    }

    protected void Update()
    {
        if (_target) TargetEnemy();
    }

    protected virtual void TargetEnemy()
    {
        if (_closestEnemy)
            transform.position =
                Vector3.MoveTowards(transform.position, _closestEnemy.position, _speed * Time.deltaTime);
    }

    public void Shoot(Vector3 direction, float force)
    {
        _rigidbody.velocity = direction.normalized * force * Time.deltaTime;
    }

    protected virtual Transform GetClosestEnemy()
    {
        _enemiesInScene = FindObjectsOfType<StickyMinion>();
        float closestDisteance = Mathf.Infinity;
        Transform targetTransform = null;

        foreach (var enemy in _enemiesInScene)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (currentDistance < closestDisteance)
            {
                closestDisteance = currentDistance;

                if (enemy.GetMinionState() == MinionState.ChasePlayer || enemy.GetMinionState() == MinionState.Spawning)
                    targetTransform = enemy.transform;
            }
        }

        return targetTransform;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}