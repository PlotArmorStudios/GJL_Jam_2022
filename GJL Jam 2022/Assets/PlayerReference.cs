using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    public static Transform Player { get;  private set; }
    private void Awake() 
    {
        Player = transform;    
    }
}
