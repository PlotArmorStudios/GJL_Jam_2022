using UnityEngine;

public class JellyShotSpawner : MonoBehaviour
{
    [SerializeField] private JellyShotAimer _jellyShotAimer;
    [SerializeField] private JellyAmmoManager _jellyAmmoManager;
    
    private void OnEnable()
    {
        _jellyShotAimer.InstantiateProjectile(transform);
        _jellyAmmoManager.SubtractAmmo();
    }
}