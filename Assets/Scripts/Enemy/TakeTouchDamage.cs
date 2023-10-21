using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTouchDamage : MonoBehaviour
{
    [SerializeField] private float touchDamage;
    [SerializeField] private float touchDamageTimerMax;
    private float touchDamageTimer;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (touchDamageTimer > touchDamageTimerMax)
            {
                other.GetComponent<PlayerController>().TakeDamage(touchDamage);
                touchDamageTimer = 0f;
            }
        }
    }

    private void Update()
    {
        touchDamageTimer += Time.deltaTime;
    }
}
