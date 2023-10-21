using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUI : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        abilityImage.fillAmount =
            Math.Min((Time.time - playerController.startTimer) / CharacterHolder.Instance.selectCharacter.coolDown, 1);
    }
}
