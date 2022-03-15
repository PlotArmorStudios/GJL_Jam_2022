using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 * Player Movement
 * use "_" in front of all private member variables to further differentiate from
 * local and public variables.
 */
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform _cam;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = 10f;

    private CharacterController _characterController;
    public Vector3 Movement;
    public Vector3 MoveDirection { get; set; }

    private Run _run;
    private GroundDetect _groundDetect;

    private float _horizontal;
    private float _vertical;

    private float turnSmoothVelocity;
    private float _verticalVelocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _run = GetComponent<Run>();
        _groundDetect = GetComponent<GroundDetect>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        //Capture horizontal and vertical movement input
        Movement = new Vector3(_horizontal, 0f, _vertical).normalized;

        if (Movement.magnitude >= .1f)
        {
            Move();
            _run.AnimateRun(true);
        }
        else
        {
            _run.AnimateRun(false);
        }

        //Having issues with ground detection no working
        //"Feet" object constantly moves after 1 jump. Y transform does not change however.
        //Jump();
    }

    private void Jump()
    {
        if (_groundDetect.ToggleGroundedState())
        {
            if (Input.GetButtonDown("Jump"))
            {
                _verticalVelocity = _jumpForce;
            }
        }
        else
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        float targetAngle = Mathf.Atan2(Movement.x, Movement.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
        float angle =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * new Vector3(0, _verticalVelocity, 1);

        _characterController.Move(MoveDirection.normalized * _speed * Time.deltaTime);
    }
}