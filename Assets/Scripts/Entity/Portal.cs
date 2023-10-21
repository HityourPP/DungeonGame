using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isPlayerAround;
    private Transform player;
    private void Start()
    {
        InputManager.Instance.OnInteraction += InputManagerOnInteraction;
    }

    private void InputManagerOnInteraction(object sender, EventArgs e)
    {
        if (isPlayerAround)
        {
            GameManager.Instance.player = player;
            SceneManager.LoadScene("Level1");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAround = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAround = false;
        }
    }
}
