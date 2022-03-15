using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyMinionPool : ObjectPool
{
    public GameObject GetObject(MinionStats stats)
    {
        GameObject minion = base.GetObject();
        minion.GetComponent<StickyMinion>().Stats = stats;
        float size = stats._size;
        minion.transform.localScale = new Vector3(size, size, size);
        return minion;
    }
}
