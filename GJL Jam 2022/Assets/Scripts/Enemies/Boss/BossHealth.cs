using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    protected override void Die()
    {
        Debug.Log("Boss died.");
    }
}
