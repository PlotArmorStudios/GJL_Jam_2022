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
    public static event Action OnToggleFreezingJelly;
    public static event Action OnToggleDamageJelly;

    [SerializeField] private Animator _animator;
    [SerializeField] private JellyToShoot _jellyToShoot = JellyToShoot.FreezingJelly;
    [SerializeField] private GameObject _damageJelly;
    [SerializeField] private GameObject _freezingJelly;
    [SerializeField] private float _fireTimeDelay = .5f;

    private Transform _currentSpawnPoint;
    private GameObject _currentJelly;
    private DamageJellyAmmoManager _ammoManager;
    private float _currentFireTime;

    public JellyToShoot JellyToShoot => _jellyToShoot;
    public GameObject CurrentJelly => _currentJelly;

    private void Start()
    {
        _ammoManager = GetComponent<DamageJellyAmmoManager>();
        ToggleJellyToShoot();
    }

    void Update()
    {
        _currentFireTime += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            if (_currentFireTime < _fireTimeDelay) return;

            if (_jellyToShoot == JellyToShoot.DamageJelly && _ammoManager.CurrentAmmo > 0)
                _animator.SetBool("Shooting", true);
            else if (_jellyToShoot != JellyToShoot.DamageJelly)
                _animator.SetBool("Shooting", true);

            _currentFireTime = 0;
        }
        else
            _animator.SetBool("Shooting", false);

        if (Input.GetKeyDown(KeyCode.F))
            ToggleJellyToShoot();
    }

    private void ToggleJellyToShoot()
    {
        if (_jellyToShoot == JellyToShoot.FreezingJelly)
        {
            _jellyToShoot = JellyToShoot.DamageJelly;
            OnToggleDamageJelly?.Invoke();
        }
        else if (_jellyToShoot == JellyToShoot.DamageJelly)
        {
            _jellyToShoot = JellyToShoot.FreezingJelly;
            OnToggleFreezingJelly?.Invoke();
        }

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