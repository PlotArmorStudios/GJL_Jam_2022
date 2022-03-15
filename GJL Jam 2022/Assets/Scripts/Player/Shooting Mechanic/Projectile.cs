using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _force;

    private Rigidbody _rigidbody;
    
    //Can be used for specific aim direction
    public Vector3 Direction { get; set; }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddRelativeForce(Vector3.forward * _force, ForceMode.Impulse);
    }
}
