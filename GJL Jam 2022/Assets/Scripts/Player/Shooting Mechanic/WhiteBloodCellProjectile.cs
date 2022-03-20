using UnityEngine;

public class WhiteBloodCellProjectile : Projectile
{
    [SerializeField] private float _damage;

    private Boss _bossInscene;

    protected override Transform GetClosestEnemy()
    {
        _bossInscene = FindObjectOfType<Boss>();
        var targetTransform = _bossInscene.transform;

        return targetTransform;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        var boss = other.gameObject.GetComponent<Boss>();

        if (boss) boss.GetComponent<Health>().TakeDamage(_damage);

        base.OnCollisionEnter(other);
    }
}