using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GroundDetect : MonoBehaviour
{
    [SerializeField] private Transform _feet;
    [SerializeField] private float _gravityForce;
    [SerializeField] private LayerMask _groundDetectLayerMask;
    [SerializeField] private float _detectDistance = 1f;

    private PlayerControl _player;
    private CharacterController _characterController;

    private void Start()
    {
        _player = GetComponent<PlayerControl>();
        _characterController = GetComponent<CharacterController>();
    }

    public bool ToggleGroundedState()
    {
        Debug.Log(Physics.Raycast(_feet.position, Vector3.down, _detectDistance, _groundDetectLayerMask));
        return Physics.Raycast(_feet.position, Vector3.down, _detectDistance, _groundDetectLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_feet.position, Vector3.down * _detectDistance);
    }
}