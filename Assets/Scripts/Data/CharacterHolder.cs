using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHolder : MonoBehaviour
{
    public static CharacterHolder Instance { get; private set; }
    public CharacterInfo_SO selectCharacter;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
