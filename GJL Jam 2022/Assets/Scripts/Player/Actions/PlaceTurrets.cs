using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlaceTurrets : MonoBehaviour
{
    [SerializeField] private float _towerPlacementDistance;
    [SerializeField] private GameObject _whiteCellTower;

    [SerializeField] private TurretAmmoManager _turretAmmoManager;

    private void Start()
    {
        _turretAmmoManager = GetComponent<TurretAmmoManager>();
    }

    private void PlaceTower()
    {
        Vector3 towerPlacementVector = transform.position + transform.forward * _towerPlacementDistance;
        var towerRotation = gameObject.transform.parent.transform.rotation;
        Instantiate(_whiteCellTower, towerPlacementVector, towerRotation * Quaternion.Euler(0, 180, 0));
        _turretAmmoManager.SubtractAmmo();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
}