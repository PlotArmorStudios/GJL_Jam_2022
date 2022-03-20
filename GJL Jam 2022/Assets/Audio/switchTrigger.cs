using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("CutsceneMusic ", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
