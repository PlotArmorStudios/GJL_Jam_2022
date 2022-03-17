using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JellyToShoot
{
    DamageJelly,
    FreezingJelly
}

public class JellyShotToggler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private JellyToShoot _jellyToShoot = JellyToShoot.FreezingJelly;
    [SerializeField] private GameObject _damageJelly;
    [SerializeField] private GameObject _freezingJelly;

    private Transform _currentSpawnPoint;
    private GameObject _currentJelly;
    private JellyAmmoManager _jellyAmmoManager;
    public GameObject CurrentJelly => _currentJelly;

    private void Start()
    {
        _jellyAmmoManager = GetComponent<JellyAmmoManager>();
        ToggleJellyToShoot();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (_jellyToShoot == JellyToShoot.DamageJelly && _jellyAmmoManager.CurrentDamageAmmo > 0)
                _animator.SetBool("Shooting", true);
            else if (_jellyToShoot != JellyToShoot.DamageJelly)
                _animator.SetBool("Shooting", true);
        }
        else
            _animator.SetBool("Shooting", false);

        if (Input.GetKeyDown(KeyCode.F))
            ToggleJellyToShoot();
    }

    private void ToggleJellyToShoot()
    {
        if (_jellyToShoot == JellyToShoot.FreezingJelly)
            _jellyToShoot = JellyToShoot.DamageJelly;
        else if (_jellyToShoot == JellyToShoot.DamageJelly)
            _jellyToShoot = JellyToShoot.FreezingJelly;

        switch (_jellyToShoot)
        {
            case JellyToShoot.DamageJelly:
                _currentJelly = _damageJelly;
                Debug.Log("Equipped with Red Jelly.");
                break;
            case JellyToShoot.FreezingJelly:
                _currentJelly = _freezingJelly;
                Debug.Log("Equipped with Blue Jelly.");
                break;
        }
    }
}