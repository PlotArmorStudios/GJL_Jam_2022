using UnityEngine;

public class FlashTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialUI;

    private void OnEnable() => GameManager.OnFlashTutorial += FlashTutorialUI;
    private void OnDisable() => GameManager.OnFlashTutorial -= FlashTutorialUI;

    private void FlashTutorialUI()
    {
        _tutorialUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DeactivateTutorialUI()
    {
        _tutorialUI.SetActive(false);
        GameManager.Instance.UnpauseGame();
    }
}