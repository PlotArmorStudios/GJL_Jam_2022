using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMinionAnimation : MonoBehaviour
{
    public static event Action OnBossThrow;
    public static event Action OnBossStopThrow;
    public void ThrowMinion() => OnBossThrow?.Invoke();
    public void StopThrowing() => OnBossStopThrow?.Invoke();
}