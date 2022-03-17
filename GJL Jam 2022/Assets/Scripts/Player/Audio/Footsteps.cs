using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _leftFootClip, _rightFootClip;
    
    private void LeftStep()
    {
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
       // _audioSource.PlayOneShot(_leftFootClip);
    }

    private void RightStep()
    {
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
        // _audioSource.PlayOneShot(_rightFootClip);
    }
}
