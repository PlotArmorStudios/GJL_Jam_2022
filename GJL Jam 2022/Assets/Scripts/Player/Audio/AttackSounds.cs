using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSouce;
    [SerializeField] private AudioClip _attackSound;

    private void PlayAttackSound()
    {
        AkSoundEngine.PostEvent("Play_Freeze_Shot", gameObject);
        //_audioSouce.PlayOneShot(_attackSound);
    }
}
