using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool ifPlayerAlive; 
    public Transform player;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        
        ifPlayerAlive = false;
        if ( SceneManager.GetActiveScene().name == "GameScene"|| SceneManager.GetActiveScene().name == "Level1")
        {
            GameObject playerGamObject = Instantiate(CharacterHolder.Instance.selectCharacter.playerGameObject,
                transform.position, Quaternion.identity);
            player = playerGamObject.transform;
            ifPlayerAlive = true;
        }
        DontDestroyOnLoad(gameObject);
    }
}
