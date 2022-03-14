using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    [SerializeField] private Animator _modelAnimator;

    public void AnimateRun(bool toggle)
    {
        _modelAnimator.SetBool("Running", toggle);
    }
}
