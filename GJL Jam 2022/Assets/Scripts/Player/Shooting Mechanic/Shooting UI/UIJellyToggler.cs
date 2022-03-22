using UnityEngine;
using UnityEngine.UI;

public class UIJellyToggler : MonoBehaviour
{
    [SerializeField] private Image _freezingJellyUI;
    [SerializeField] private Image[] _damageJellyUI;

    private DamageJellyAmmoManager _damageJellyAmmoManager;
    
    private void OnEnable()
    {
        JellyShotToggler.OnToggleDamageJelly += ToggleDamageJellyUI;
        JellyShotToggler.OnToggleFreezingJelly += ToggleFreezingJellyUI;
        
        _damageJellyAmmoManager = GetComponent<DamageJellyAmmoManager>();
    }

    private void OnDisable()
    {
        JellyShotToggler.OnToggleDamageJelly -= ToggleDamageJellyUI;
        JellyShotToggler.OnToggleFreezingJelly -= ToggleFreezingJellyUI;
    }
    private void ToggleFreezingJellyUI()
    {
        _freezingJellyUI.color = new Color(1, 1, 1, 1f);
        
        foreach (var damageJellyUI in _damageJellyUI)
        {
            damageJellyUI.color = new Color(1, 1, 1, .3f);
        }

    }

    private void ToggleDamageJellyUI()
    {
        _freezingJellyUI.color = new Color(1, 1, 1, .3f);

        for (int i = 0; i < _damageJellyAmmoManager.CurrentAmmo; i++)
        {
            _damageJellyUI[i].color = new Color(1, 1, 1, 1f);
        }
    }
}