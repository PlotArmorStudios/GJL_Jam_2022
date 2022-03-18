using UnityEngine;

public class WhiteBloodCellProjectile : Projectile
{
    private Boss _bossInscene;

    public override Transform GetClosestEnemy()
    {
        _bossInscene = FindObjectOfType<Boss>();
        var targetTransform = _bossInscene.transform;

        return targetTransform;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        var boss = other.gameObject.GetComponent<Boss>();
        if (!boss) return;
        //Do damage to boss
    }
}