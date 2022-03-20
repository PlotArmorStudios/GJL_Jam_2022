using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // AkSoundEngine.SetSwitch("Music","CutsceneMusic", gameObject);

        mySwitch.SetValue(this.gameObject);
    }

  

    [SerializeField]
    private AK.Wwise.Switch mySwitch;
    

    }