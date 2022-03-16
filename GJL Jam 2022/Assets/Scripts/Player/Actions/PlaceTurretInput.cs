using UnityEngine;

public class PlaceTurretInput : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger("Place Tower");
        }
    }
}