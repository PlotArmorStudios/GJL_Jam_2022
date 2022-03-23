using System;
using UnityEngine;

public class PlaceTurretInput : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _inputDelay = .5f;

    private TurretAmmoManager _turretAmmoManager;
    private float _currentInputTime;

    private void Start()
    {
        _turretAmmoManager = GetComponent<TurretAmmoManager>();
    }

    private void Update()
    {
        _currentInputTime += Time.deltaTime;

        if (_currentInputTime >= _inputDelay)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_turretAmmoManager.CurrentAmmo > 0)
                {
                    _turretAmmoManager.SubtractAmmo();
                    _animator.SetTrigger("Place Tower");
                    AkSoundEngine.PostEvent("Play_White_Cell_Tower_Spawn", gameObject);
                    _currentInputTime = 0;
                }
            }
        }
    }
}