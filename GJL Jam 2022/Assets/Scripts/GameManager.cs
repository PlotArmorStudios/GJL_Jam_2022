using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _playerRespawnTime;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoseGame()
    {
        
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