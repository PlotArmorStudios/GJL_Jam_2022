using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : Health
{
    [Tooltip("Scene to load on death.")]
    [SerializeField] private string _sceneToLoad;

    protected override void Die()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
