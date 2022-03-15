using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyMinionPool : ObjectPool
{
    public GameObject GetObject(MinionStats stats, int maxHealth)
    {
        GameObject minion = base.GetObject();
        minion.GetComponent<StickyMinion>().Stats = stats;
        minion.GetComponent<EnemyHealth>().SetNewMaxHealth(maxHealth);
        float size = stats._size;
        minion.transform.localScale = new Vector3(size, size, size);
        return minion;
    }
}
