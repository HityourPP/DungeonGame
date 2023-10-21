using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Abilities : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerStats playerStats;
    private CharacterInfo_SO selectedCharacter;

    public UnityEvent[] abilities;
    public List<bool> activeAbilities;
    public int selectAbility;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerStats = GetComponentInParent<PlayerStats>();

        selectedCharacter = CharacterHolder.Instance.selectCharacter;
    }

    public void RapidFire() //快速射击
    {
        if (!activeAbilities[selectAbility])
        {
            activeAbilities[selectAbility] = true;
            playerStats.fireRate += 1f;
        }
        else
        {
            activeAbilities[selectAbility] = false;
            playerStats.fireRate -= 1f;
        }
    }

    public void Heal()  //治疗
    {   //设置回复HP
        playerController.SetCurrentHealth(playerController.GetCurrentHealth() + 10);
        playerController.RefreshUI();
    }

    public void ArmorUp()   //增加护甲
    {
        if (!activeAbilities[selectAbility])
        {
            activeAbilities[selectAbility] = true;
            playerStats.armorStat += 1f;
        }
        else
        {
            activeAbilities[selectAbility] = false;
            playerStats.armorStat -= 1f;
        }
    }

    public void ActivateCoroutine()
    {
        StartCoroutine(ActivateAbility());
    }
    
    IEnumerator ActivateAbility()
    {        
        playerController.isAbilityReleasing = true;
        abilities[selectAbility].Invoke();
        yield return new WaitForSeconds(selectedCharacter.abilityDuration);     //设置技能持续时间
        abilities[selectAbility].Invoke();
        playerController.isAbilityReleasing = false;
    }
}
