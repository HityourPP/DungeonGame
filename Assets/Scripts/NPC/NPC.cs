using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject tip;
    [SerializeField] private GameObject takeUI;
    private bool ifTalkOver;
    private void Start()
    {
        tip.SetActive(false);
        takeUI.SetActive(false);
        InputManager.Instance.OnTalk += InputManagerOnTalk;
    }

    private void InputManagerOnTalk(object sender, EventArgs e)
    {
        if (tip.activeSelf && !ifTalkOver)
        {
            ifTalkOver = true;
            takeUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tip.SetActive(false);
        }
    }
    
}
