using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private void LeftStep()
    {
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }

    private void RightStep()
    {
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }
}
