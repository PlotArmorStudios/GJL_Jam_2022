using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlaceTurrets : MonoBehaviour
{
    [SerializeField] private float _towerPlacementDistance;
    [SerializeField] private GameObject _whiteCellTower;

    private void PlaceTower()
    {
        Vector3 towerPlacementVector = transform.position + transform.forward * _towerPlacementDistance;
        var towerRotation = transform.rotation;
        towerRotation.y += 180;
        Instantiate(_whiteCellTower, towerPlacementVector, towerRotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
}