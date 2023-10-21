using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float currentHealth;
    public bool isHurt;
    private float isHurtTimer;
    private void Awake()
    {
        currentHealth = maxHealth;
        isHurt = false;
    }

    private void Update()
    {
        isHurtTimer += Time.deltaTime;
        if (isHurtTimer > 1f)
        {
            isHurtTimer = 0f;
            isHurt = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        isHurt = true;
    }
    
}
