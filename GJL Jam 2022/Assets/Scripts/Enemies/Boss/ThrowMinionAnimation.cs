using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMinionAnimation : MonoBehaviour
{
    public static event Action OnBossThrow;
    public void ThrowMinion() => OnBossThrow?.Invoke();
}