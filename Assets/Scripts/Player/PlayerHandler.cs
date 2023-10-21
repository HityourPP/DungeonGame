using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private CharacterInfo_SO selectCharacter;
    private PlayerController playerController;

    private void Start()
    {
        selectCharacter = CharacterHolder.Instance.selectCharacter;
        playerController = GetComponent<PlayerController>();
        SetCharacter();
    }

    private void SetCharacter()
    {
        playerController.weaponData = selectCharacter.weapon;
        GameObject spawnAbility = Instantiate(selectCharacter.ability, transform.position, Quaternion.identity);
        GameObject spawnPassive = Instantiate(selectCharacter.passive, transform.position, Quaternion.identity);
        spawnAbility.transform.parent = gameObject.transform;
        spawnPassive.transform.parent = gameObject.transform;
    }
}