using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSound : MonoBehaviour
{
    public void Press()
    {
        AkSoundEngine.PostEvent("Play_UI_gamestart_or_reset", gameObject);
    }
}