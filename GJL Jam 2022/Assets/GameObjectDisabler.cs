using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisabler : MonoBehaviour
{
    [SerializeField] private GameObject[] _toEnable;
    [SerializeField] private GameObject[] _toDisable;
    public void DisableGameObject()
    {
        foreach (GameObject g in _toEnable)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in _toDisable)
        {
            g.SetActive(false);
        }
    }
}
