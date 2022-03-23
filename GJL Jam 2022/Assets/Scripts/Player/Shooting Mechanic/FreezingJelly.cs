using UnityEngine;

public class FreezingJelly : Projectile
{
    protected override void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("BodyPart")) return;
        if (other.gameObject.CompareTag("Player")) return;

        var minion = other.gameObject.GetComponent<StickyMinion>();
        if (minion)
        {
            minion.Freeze();
            Destroy(gameObject);
        }
    }
}