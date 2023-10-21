using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float touchDamage;
    [SerializeField] private float damageTimerMax;
    private float damageTimer;
    private void Update()
    {
        damageTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (damageTimer >= damageTimerMax)
            {
                other.GetComponent<PlayerController>().TakeDamage(touchDamage);
                damageTimer = 0f;
            }
        }
    }

    private void DestroySelf()
    {
        gameObject.SetActive(false);
    }
}
