using UnityEngine;

public class JellyShotSpawner : MonoBehaviour
{
    [SerializeField] private JellyShotAimer _jellyShotAimer;
    
    private void OnEnable()
    {
        _jellyShotAimer.InstantiateProjectile(transform);
    }
}