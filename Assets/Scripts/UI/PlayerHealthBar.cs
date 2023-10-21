using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    private PlayerController playerController;
    private Image healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }

    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        playerController.OnPlayerHealthBar += PlayerOnPlayerHealthBar;
        playerImage.sprite = CharacterHolder.Instance.selectCharacter.sprites[0];
    }

    private void PlayerOnPlayerHealthBar(object sender, PlayerController.PlayerFillAmountNormalize e)
    {
        healthBar.fillAmount = e.fillAmountNormalize;
    }
}
