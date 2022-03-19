using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    [SerializeField] private GameObject _objectToToggle;

    public void ToggleObject()
    {
        _objectToToggle.SetActive(false);
    }
}
