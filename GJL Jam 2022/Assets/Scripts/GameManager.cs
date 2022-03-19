using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _playerRespawnTime;
    
    public static GameManager Instance;
    private PlayerControl _player;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        Instance = this;
        _player = FindObjectOfType<PlayerControl>();
        _sceneLoader = GetComponent<SceneLoader>();
    }

    public void LoseGame()
    {
        _sceneLoader.LoadScene();
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
}