using System;
using UnityEngine;

public class PlaceTurretInput : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private TurretAmmoManager _turretAmmoManager;

    private void Start()
    {
        _turretAmmoManager = GetComponent<TurretAmmoManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger("Place Tower");
        }
    }
}