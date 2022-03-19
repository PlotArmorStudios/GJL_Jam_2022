using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _playerRespawnTime;
    
    private PlayerControl _player;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        _player = FindObjectOfType<PlayerControl>();
    }

    public void LoseGame()
    {
        Debug.Log("Lost Game.");
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