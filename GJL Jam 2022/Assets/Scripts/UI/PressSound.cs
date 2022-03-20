using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSound : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClipName;
    [SerializeField] private AudioSource _audioSourceName;
    public void Press()
    {
        _audioSourceName.PlayOneShot(_audioClipName);
        AkSoundEngine.PostEvent("Play_UI_gamestart_or_reset", gameObject);
    }
}