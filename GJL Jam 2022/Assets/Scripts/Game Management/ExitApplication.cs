using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitApplication : MonoBehaviour
{
    public void Quit()
    {
        StartCoroutine(DelayQuit());
    }

    private IEnumerator DelayQuit()
    {
        var buttons = FindObjectsOfType<Button>();

        foreach (var button in buttons)
        {
            if (button != this.GetComponent<Button>())
                button.interactable = false;
        }

        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}