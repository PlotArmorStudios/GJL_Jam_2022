using UnityEngine;

public class DamageJelly : Projectile
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BodyPart")) return;
        if (other.gameObject.CompareTag("Player")) return;

        var minion = other.gameObject.GetComponent<StickyMinion>();

        if (!minion) return;

        minion.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}