using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Transform bullet;
    [SerializeField] private Sprite bulletImage;
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float blastRadius;
    private EnemyBullet bulletScript;

    private void Start()
    {
        bullet.GetComponent<SpriteRenderer>().sprite = bulletImage;
    }

    private void OnEnable()
    {
        for (int i = 0; i < 18; i++)
        {
            Transform spawnerBullet = Instantiate(bullet, transform.position,
                Quaternion.AngleAxis(i * 20, Vector3.forward));
            bulletScript = spawnerBullet.GetComponent<EnemyBullet>();
            bulletScript.bulletSpeed = bulletSpeed;
            bulletScript.damage = damage;
            bulletScript.lifeTime = lifeTime;
            bulletScript.blastRadius = blastRadius;
        }
    }

}