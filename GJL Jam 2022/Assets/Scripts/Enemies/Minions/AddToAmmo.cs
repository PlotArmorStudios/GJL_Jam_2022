using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToAmmo : MonoBehaviour
{
    public static event Action OnMinionFreeze;
    
    public void OnFreezeMinion()
    {
        OnMinionFreeze?.Invoke();
    }
}
