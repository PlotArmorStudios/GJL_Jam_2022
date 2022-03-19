using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : Health
{
    protected override void Die()
    {
        Debug.Log("Boss died");
        GameManager.Instance.WinGame();
    }
}
