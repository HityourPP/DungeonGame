using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManaBar : MonoBehaviour
{
    private PlayerController playerController;
    private Image manaBar;

    private void Awake()
    {
        manaBar = GetComponent<Image>();
    }

    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        playerController.OnPlayerManaBar += PlayerOnPlayerManaBar;
    }

    private void PlayerOnPlayerManaBar(object sender, PlayerController.PlayerFillAmountNormalize e)
    {
        manaBar.fillAmount = e.fillAmountNormalize;
    }
}
