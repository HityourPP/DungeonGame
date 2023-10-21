using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Passive : MonoBehaviour
{
    public UnityEvent[] passive;
    public int[] selectPassive;
    
    private PlayerStats playerStats;
    private PlayerController playerController;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerController = GetComponentInParent<PlayerController>();
        for (int i = 0; i < selectPassive.Length; i++)
        {
            passive[selectPassive[i]].Invoke();
        }
    }
    public void FireUp()    //增加攻速
    {
        playerStats.fireRate += 0.3f;
    }

    public void HealthUp()  //增加血量
    {
        playerStats.maxHealth *= 1.1f;
        playerController.SetCurrentHealth(playerStats.maxHealth);
    }

    public void DefenseUp() //增加防御
    {
        playerStats.armorStat += 0.2f;
    }

    public void ExplosionRadiusUp() //增加子弹爆炸半径
    {
        playerStats.blastRadiusStat += 1f;
    }
}
