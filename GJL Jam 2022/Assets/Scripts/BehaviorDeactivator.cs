using UnityEngine;

public class BehaviorDeactivator : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnGameEnd += DeactivateBehaviors;
    }

    public void DeactivateBehaviors()
    {
        var behaviours = GetComponents<Behaviour>();
        
        foreach (var behaviour in behaviours)
        {
            behaviour.enabled = false;
        }
    }
    
    private void OnDisable()
    {
        GameManager.OnGameEnd -= DeactivateBehaviors;
    }
}