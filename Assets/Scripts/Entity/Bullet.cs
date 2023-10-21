using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;
    public float lifeTime;
    public float bulletSpeed;
    public float blastRadius;

    public LayerMask enemy;
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
            
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, blastRadius, enemy);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damage);   //对范围内的敌人造成伤害
            }
        }
        else if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
