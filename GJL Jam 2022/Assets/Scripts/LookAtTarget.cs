/*
Copyright JACPro 2022 - https://jacpro.github.io
GJL Game Parade Spring 2022 - https://itch.io/jam/game-parade-spring-2022
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] Transform _target;

    private void Awake() 
    {
        if (_target == null)
        {
            _target = Camera.main.transform;
        }    
    }

    private void Update()
    {
        transform.LookAt(_target);
    }
}
