using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Weapons_SO weaponData;
    private Weapons_SO weaponTemp;
    private SpriteRenderer weaponImage;
    private bool isPlayerAround;
    private PlayerController player;
    private AudioSource weaponChangedAudio;
    private void Awake()
    {
        weaponImage = GetComponent<SpriteRenderer>();
        weaponChangedAudio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        weaponImage.sprite = weaponData.weaponImage;
        InputManager.Instance.OnInteraction += InputManagerOnInteraction;
        player = GameManager.Instance.player.GetComponent<PlayerController>();
    }

    private void InputManagerOnInteraction(object sender, EventArgs e)
    {
        if (isPlayerAround)
        {
            //切换武器
            weaponChangedAudio.Play(); //设置切换武器音效
            weaponTemp = player.weaponData;
            player.weaponData = weaponData;
            weaponData = weaponTemp;
            player.RefreshWeaponImage();
            weaponImage.sprite = weaponData.weaponImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAround = true;
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
