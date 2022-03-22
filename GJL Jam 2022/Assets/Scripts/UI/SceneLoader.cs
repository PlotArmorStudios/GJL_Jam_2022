using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private Image _fadeScreen;
    [SerializeField] private bool _transitionToggle;
    [SerializeField] private float _transitionSpeed = 1f;

    public void LoadScene()
    {
        if (_transitionToggle)
            StartCoroutine(PlayTransition(_sceneToLoad));
        else
            TransitionScene(_sceneToLoad);
    }

    public void LoadScene(string scene)
    {
        if (_transitionToggle)
            StartCoroutine(PlayTransition(scene));
        else
            TransitionScene(scene);
    }

    public void LoadScene(string scene, bool transitionToggle)
    {
        if (transitionToggle)
            StartCoroutine(PlayTransition(scene));
        else
            TransitionScene(scene);
    }

    private IEnumerator PlayTransition(string scene)
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += .01f * _transitionSpeed;
            var newColor = new Color(0, 0, 0, alpha);
            _fadeScreen.color = newColor;
            yield return null;
        }

        TransitionScene(scene);
    }

    private void TransitionScene(string scene)
    {
        if (GameManager.Instance)
            GameManager.Instance.UnpauseGame();
        SceneManager.LoadScene(scene);
    }
}