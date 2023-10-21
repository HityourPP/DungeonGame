using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private bool gamePausing;
    private void Awake()
    {
        resumeButton.onClick.AddListener((() =>
        {
            gamePausing = false;
            Hide();
        }));
        mainMenuButton.onClick.AddListener((() =>
        {
            SceneManager.LoadScene("MainMenu");
        }));
    }

    private void Start()
    {
        InputManager.Instance.OnPauseGame+= InputManagerOnPauseGame;
        Hide();
    }

    private void InputManagerOnPauseGame(object sender, EventArgs e)
    {
        gamePausing = !gamePausing;
        if (gamePausing)
        {
            Show();
        }
        else
        {
            Hide(); 
        }
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
}
