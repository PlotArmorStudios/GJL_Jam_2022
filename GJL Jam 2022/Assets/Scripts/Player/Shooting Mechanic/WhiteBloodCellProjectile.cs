using UnityEngine;

public class WhiteBloodCellProjectile : Projectile
{
    [SerializeField] private float _damage;

    private Boss _bossInscene;

    
    protected virtual void TargetEnemy()
    {
        if (_closestEnemy)
            transform.position =
                Vector3.MoveTowards(transform.position, _closestEnemy.position, _speed * Time.deltaTime);
    }
    
    protected override Transform GetClosestEnemy()
    {
        _bossInscene = FindObjectOfType<Boss>();
        var targetTransform = _bossInscene.transform;

        return targetTransform;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        var boss = other.gameObject.GetComponent<Boss>();
        var tower = other.gameObject.GetComponent<WhiteCellTower>();

        if (boss)
        {
            Debug.Log("Hit boss");
            boss.GetComponent<Health>().TakeDamage(_damage);
        }
        if (tower) return;
        
        base.OnCollisionEnter(other);
    }
}