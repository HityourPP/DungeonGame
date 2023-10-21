using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEffect : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private bool isEnemyBullet;
    public float enemyBulletSize;
    private void Start()
    {
        if (!isEnemyBullet && GameManager.Instance.ifPlayerAlive)
        {        
            playerStats = GameManager.Instance.player.GetComponent<PlayerStats>();
            playerStats = FindObjectOfType<PlayerStats>();
            transform.localScale *=
                GameManager.Instance.player.GetComponent<PlayerController>().weaponData.blastRadius *
                (1 + playerStats.blastRadiusStat) * 3f;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, CharacterHolder.Instance.selectCharacter.weapon.blastRadius *
    //                                               (1 + playerStats.blastRadiusStat));
    // }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
