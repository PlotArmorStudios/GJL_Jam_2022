using UnityEngine;
using UnityEngine.Serialization;

public class JellyShotSpawner : MonoBehaviour
{
    [SerializeField] private JellyShotAimer _jellyShotAimer;
    [SerializeField] private AmmoManager ammoManager;
    
    private void OnEnable()
    {
        _jellyShotAimer.InstantiateProjectile(transform);
        ammoManager.SubtractAmmo();
    }
}