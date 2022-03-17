using System;
using UnityEngine;

public class JellyShotAimer : MonoBehaviour
{
    [SerializeField] private JellyShotToggler jellyShotToggler;
    [SerializeField] private float _force;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _defaultRayRange = 1000;

    private Vector3 _destination;
    private Vector3 _direction;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CalculateAim();
        }
    }

    public void CalculateAim()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            _destination = hit.point;
        }

        _direction = _destination - transform.position;
    }

    public void InstantiateProjectile(Transform spawnPoint)
    {
        var jelly = Instantiate(jellyShotToggler.CurrentJelly, spawnPoint.position, spawnPoint.rotation);
        jelly.GetComponent<Projectile>().Shoot(_direction, _force);
    }
}