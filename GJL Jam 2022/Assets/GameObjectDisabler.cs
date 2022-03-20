using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisabler : MonoBehaviour
{
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
