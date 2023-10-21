using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaClump : MonoBehaviour
{
    public float heldMana;
    [SerializeField] private float speed = 10f;
    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void Update()
    {
        if (GameManager.Instance.ifPlayerAlive)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                player.position, speed * Time.deltaTime);   //向玩家位置移动            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.GainMana(heldMana);
            Destroy(gameObject);
        }
    }
}
