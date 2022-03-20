using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _playerRespawnTime;
    [SerializeField] private string _winScene, _loseScene;

    public static GameManager Instance;
    private PlayerControl _player;
    private SceneLoader _sceneLoader;
    
    public bool GamePaused { get; private set; }

    public static event Action OnGamePause;
    public static event Action OnGameUnpause;
    public static event Action OnGameEnd;

    private void Awake()
    {
        Instance = this;
        _player = FindObjectOfType<PlayerControl>();
        _sceneLoader = GetComponent<SceneLoader>();
        GamePaused = false;
    }

    public void WinGame()
    {
        OnGameEnd?.Invoke();
        _sceneLoader.LoadScene(_winScene, true);
    }

    public void LoseGame()
    {
        OnGameEnd?.Invoke();
        _sceneLoader.LoadScene(_loseScene, false);
    }

    public void DespawnPlayer()
    {
        StartCoroutine(TogglePlayer());
    }

    private IEnumerator TogglePlayer()
    {
        _player.gameObject.SetActive(false);
        yield return new WaitForSeconds(_playerRespawnTime);
        _player.gameObject.SetActive(true);
        _player.GetComponent<FlashOnRespawn>().Flash();
    }

    public void PauseGame()
    {
        OnGamePause?.Invoke();
        GamePaused = true;
    }

    public void UnpauseGame()
    {
        OnGameUnpause?.Invoke();
        GamePaused = false;
    }

}