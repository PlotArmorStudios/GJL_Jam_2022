using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.Instance.GamePaused)
                GameManager.Instance.PauseGame();
            else
                GameManager.Instance.UnpauseGame();
        }
    }
}