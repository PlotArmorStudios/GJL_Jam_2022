using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private PlayerControl _player;
    private GroundDetect _groundDetect;

    private void Start()
    {
        _player = GetComponent<PlayerControl>();
        _groundDetect = GetComponent<GroundDetect>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_groundDetect.ToggleGroundedState())
            {
                _player.MoveDirection = new Vector3(_player.MoveDirection.x, _jumpForce, _player.MoveDirection.y);
                _player.GetComponent<CharacterController>().Move(_player.MoveDirection);
            }
        }
    }
}