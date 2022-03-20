using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
    public void DisableGameObject()
    {
        gameObject.SetActive(false);    
    }
}
