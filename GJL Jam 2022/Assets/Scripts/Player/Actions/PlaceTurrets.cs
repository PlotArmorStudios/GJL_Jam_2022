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
        Vector3 towerPlacementVector = transform.position;
        towerPlacementVector.z += _towerPlacementDistance;
        Instantiate(_whiteCellTower, towerPlacementVector, Quaternion.identity);
    }
}