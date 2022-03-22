using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    private Vector3 _position;

    private void Awake() 
    {
        _position = transform.localPosition;    
    }

    private void Update()
    {
        if(transform.localPosition != _position) transform.localPosition = _position;
    }
}
