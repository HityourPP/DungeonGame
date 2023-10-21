using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        MoveToPlayer,
        Attack,
        Hurt,
        Death
    }
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyDamage;
    
    private EnemyState state;
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyDrops enemyDrops;
    private EnemyHealth enemyHealth;
    
    private float idleTimer;
    private float moveTimer;
    private float attackTimer;

    [SerializeField]
    protected float idleTimerMax = 3f;
    [SerializeField]
    protected float moveTimerMax = 1.5f;   
    [SerializeField]
    protected float attackTimerMax = 1f;
    private bool isLeft;

    //动画相关
    private bool isIdling;
    private bool isWalking;
    private bool isDead;
    private bool dead;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyDrops = GetComponent<EnemyDrops>();
        enemyHealth = GetComponent<EnemyHealth>();
        isLeft = false;
        dead = false;
    }

    private void Start()
    {
        state = EnemyState.Idle;
    }

    private void Update()
    {
        if (!dead)
        {
            // Debug.Log(state);
            if(enemyHealth.currentHealth <= 0)
            {
                state = EnemyState.Death;
                dead = true;
            }
            if (enemyHealth.isHurt)
            {
                state = EnemyState.Hurt;
            }
        }
        else
        {
            state = EnemyState.Death;
        }
        SwitchState();
        SwitchAnim();
    }

    private void SwitchState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.MoveToPlayer:
                MoveToPlayerState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Hurt:
                rb.velocity = Vector2.zero;
                isIdling = false;
                isWalking = false;
                anim.SetTrigger("hurt");
                enemyHealth.isHurt = false;
                state = EnemyState.Idle;
                break;
            case EnemyState.Death:
                rb.velocity = Vector2.zero;
                isIdling = false;
                isWalking = false;
                isDead = true;
                break;
        }
    }

    private void SwitchAnim()
    {
        anim.SetBool("idle",isIdling);
        anim.SetBool("walk",isWalking);
        anim.SetBool("dead",isDead);
    }

    private void IdleState()
    {
        isIdling = true;
        isWalking = false;
        rb.velocity = Vector2.zero;
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTimerMax)
        {
            state = EnemyState.MoveToPlayer;
            moveTimer = 0f;
        }
    }

    private void MoveToPlayerState()
    {
        isIdling = false;
        isWalking = true;
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveTimerMax)
        {
            state = EnemyState.Idle;
            idleTimer = 0f;
        }
        if (GameManager.Instance.ifPlayerAlive)
        {
            Vector2 direction = GameManager.Instance.player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //设置方向
            if ((angle > 90f && angle < 180f) || (angle > -180f && angle < -90f))
            {
                if (!isLeft)
                {
                    isLeft = true;
                    transform.Rotate(0, 180f, 0);
                }
            }
            if ((angle > -90f && angle < 90f))
            {
                if (isLeft)
                {
                    isLeft = false;
                    transform.Rotate(0, 180f, 0);      
                }
            }
            rb.velocity = direction.normalized * enemySpeed;
        }
    }

    private void AttackState()
    {
        //进入攻击状态先让其进入idle
        isIdling = true;
        isWalking = false;
        rb.velocity = Vector2.zero;
        idleTimer = 0f;
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackTimerMax)
        {
            isIdling = false;
            anim.SetTrigger("attack");
            attackTimer = 0f;
        }
        // anim.SetTrigger("attack");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            state = EnemyState.Attack;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
           state = EnemyState.Idle;
        }
    }

    private void Dead()
    {
        enemyDrops.DropManaClump();
        Destroy(gameObject);
    }
    private void Attack()
    {
        if (GameManager.Instance.ifPlayerAlive)
        {
            GameManager.Instance.player.GetComponent<PlayerController>().TakeDamage(enemyDamage);
        }
    }
    
}
