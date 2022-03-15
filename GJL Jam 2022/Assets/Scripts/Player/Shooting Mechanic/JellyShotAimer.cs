using System;
using UnityEngine;

public class JellyShotAimer : MonoBehaviour
{
    [SerializeField] private JellyShotToggler jellyShotToggler;
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
        Ray ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _destination = hit.point;
        }
        else
        {
            _destination = ray.GetPoint(_defaultRayRange);
        }

        _direction = transform.position - _destination;
        
    }

    public void InstantiateProjectile(Transform spawnPoint)
    {
        Instantiate(jellyShotToggler.CurrentJelly, spawnPoint.position, spawnPoint.rotation);
        jellyShotToggler.CurrentJelly.GetComponent<Projectile>().Direction = _direction;
    }
}