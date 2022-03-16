using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSouce;
    [SerializeField] private AudioClip _attackSound;

    private void PlayAttackSound()
    {
        _audioSouce.PlayOneShot(_attackSound);
    }
}
