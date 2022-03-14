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
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private Transform _cam;

    private CharacterController _characterController;

    private float _horizontal;
    private float _vertical;
    private Vector3 _movement;

    private float turnSmoothVelocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        //Capture horizontal and vertical movement input
        _movement = new Vector3(_horizontal, 0f, _vertical).normalized;

        if (_movement.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg * _cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            //Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            //keep movement direction Y-Axis independent for jumping.
            //moveDirection.y = moveDirection.y;
            _characterController.Move(_movement.normalized * _speed * Time.deltaTime);
        }
    }
}