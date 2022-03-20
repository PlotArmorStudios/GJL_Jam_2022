using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    private void Update()
    {
        if(transform.localPosition != Vector3.zero) transform.localPosition = Vector3.zero;
    }
}
