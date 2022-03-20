using UnityEngine;

public class BehaviorDeactivator : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnGameEnd += DeactivateBehaviors;
        GameManager.OnGamePause += DeactivateBehaviors;
        GameManager.OnGameUnpause += ReactivateBehaviors;
    }

    public void DeactivateBehaviors()
    {
        var behaviours = GetComponents<Behaviour>();

        Debug.Log("Deactivate behaviors");

        foreach (var behaviour in behaviours)
        {
            if (behaviour is BehaviorDeactivator) return;
            if (behaviour is Run) behaviour.GetComponent<Run>().AnimateRun(false);
            behaviour.enabled = false;
        }
    }

    public void ReactivateBehaviors()
    {
        var behaviours = GetComponents<Behaviour>();
        
        Debug.Log("Reactive behaviors");
        
        foreach (var behaviour in behaviours)
        {
            behaviour.enabled = true;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameUnpause -= ReactivateBehaviors;
        GameManager.OnGamePause -= DeactivateBehaviors;
        GameManager.OnGameEnd -= DeactivateBehaviors;
    }
}