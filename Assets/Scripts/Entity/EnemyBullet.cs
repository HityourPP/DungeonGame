using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;
    public float lifeTime;
    public float bulletSpeed;
    public float blastRadius;

    public LayerMask player;
    public GameObject blastEffect;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO:设置爆炸效果
        if (blastRadius > 0.3f)     //爆炸范围在大于0.3时实现爆炸效果
        {
            GameObject spawnEffect = Instantiate(blastEffect, transform.position, Quaternion.identity);
            spawnEffect.transform.localScale *= blastRadius * 3;
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, blastRadius, player);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<PlayerController>().TakeDamage(damage);   //对范围内的玩家造成伤害
            }
        }
        else if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}