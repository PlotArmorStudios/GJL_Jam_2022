using System.Collections;
using UnityEngine;

public class FlashOnRespawn : MonoBehaviour
{
    [SerializeField] private float _maxFlashTime;
    [SerializeField] private MeshRenderer _playerMeshRenderer;
    [SerializeField] private Material _meshMaterial;
    [SerializeField] private Material _transparentMaterial;
    
    [SerializeField] private float _amplitude;
    [SerializeField] private float _frequency;
    
    private float _currentFlashTime;

    [ContextMenu("Flash Renderer")]
    public void Flash()
    {
        _currentFlashTime = _maxFlashTime;
        StartCoroutine(FluctuateMaterialAlpha());
    }

    private IEnumerator FluctuateMaterialAlpha()
    {
        _playerMeshRenderer.material = _transparentMaterial;
        
        while (_currentFlashTime > 0)
        {
            _currentFlashTime -= Time.deltaTime;
            var materialColor = _playerMeshRenderer.material.color;
            materialColor.a = Mathf.Sin(Time.time * _frequency) * _amplitude;
            _playerMeshRenderer.material.color = materialColor;
            yield return null;
        }

        _playerMeshRenderer.material = _meshMaterial;
    }
}