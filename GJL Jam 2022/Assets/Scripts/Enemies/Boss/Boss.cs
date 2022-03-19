using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform _minionSpawnPoint;
    [SerializeField] private MinionSpawner _minionSpawner;
    [SerializeField, Tooltip("The height above spawn at which the thrown minion which reach its peak")] private float _throwHeight = 20f;

    public UnityEvent OnMinionThrown;

    /*
    Called by Animation event
    */

    [ContextMenu("Throw Minion")]
    public void ThrowMinion()
    {
        GameObject minion = _minionSpawner.SpawnStickyMinion(_minionSpawnPoint);
        minion.GetComponent<StickyMinion>().JumpAtPlayer(_throwHeight);
        OnMinionThrown.Invoke();
    }
}
