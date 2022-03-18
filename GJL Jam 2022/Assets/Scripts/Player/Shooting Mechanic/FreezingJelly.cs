using UnityEngine;

public class FreezingJelly : Projectile
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BodyPart")) return;
        if (other.gameObject.CompareTag("Player")) return;

        var minion = other.gameObject.GetComponent<StickyMinion>();

        if (!minion) return;
        
        minion.Freeze();
        Destroy(gameObject);
    }
}