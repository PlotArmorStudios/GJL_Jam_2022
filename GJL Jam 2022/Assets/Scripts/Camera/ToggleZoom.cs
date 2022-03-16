using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleZoom : MonoBehaviour
{
    [SerializeField] private CinemachineCameraOffset _vCamOffset;
    [SerializeField] private float _minOffset = 2f;
    [SerializeField] private float _maxOffset = 10f;
    
    private float _currentScroll;

    private void Update()
    {
        _currentScroll = Input.GetAxis("Mouse ScrollWheel");
        _vCamOffset.m_Offset.z += _currentScroll;
        
        if (_vCamOffset.m_Offset.z >= _maxOffset)
            _vCamOffset.m_Offset.z = _maxOffset;
        if (_vCamOffset.m_Offset.z <= _minOffset)
            _vCamOffset.m_Offset.z = _minOffset;
    }
}
