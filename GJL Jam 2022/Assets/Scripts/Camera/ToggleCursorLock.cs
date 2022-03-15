using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCursorLock : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }
}
