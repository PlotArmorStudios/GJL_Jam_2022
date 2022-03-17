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
        //_rigidbody.AddRelativeForce(Direction * _force, ForceMode.Impulse);
    }

    public void Shoot(Vector3 direction, float force)
    {
        Debug.Log(direction);
        _rigidbody.velocity = direction.normalized * force * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}
