using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootJellyShots : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
        }    
    }
}
