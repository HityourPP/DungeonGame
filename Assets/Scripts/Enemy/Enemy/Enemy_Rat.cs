using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rat : Enemy
{
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyDrops enemyDrops;
    private EnemyHealth enemyHealth;
    [SerializeField]
    private BossState bossState,preState;
    private bool isLeft;
    //动画相关
    private bool isIdling;
    private bool isWalking;
    private bool isDead;
    private bool isHurt;
    //设置状态时间
    private float idleTimer;
    private float walkTimer;
    private float touchDamageTimer;
    private float hurtTimer;
    private float ifCanHurt;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float touchDamage; 
    
    [SerializeField] private float idleTimerMax = 2f;
    [SerializeField] private float walkTimerMax = 1.5f;   
    [SerializeField] private float hurtTimerMax = 0.45f;   
    [SerializeField] private float ifCanHurtTimerMax = 2f;   
    [SerializeField] private float touchDamageTimerMax;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyDrops = GetComponent<EnemyDrops>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        preState = BossState.Idle;
        SwitchState(bossState, BossState.Idle);
    }

    private void Update()
    {
        switch (bossState)
        {
            case BossState.Idle:
                UpdateIdleState();
                break;
            case BossState.Walk:
                UpdateWalkState();
                break;
            case BossState.Hurt:
                UpdateHurtState();
                break;
        }
        SwitchAnim();
        if (enemyHealth.currentHealth <= 0)
        {
            SwitchState(bossState, BossState.Dead);
        }

        if (enemyHealth.isHurt)
        {
            if (ifCanHurt > ifCanHurtTimerMax)
            {
                ifCanHurt = 0f;
                SwitchState(bossState, BossState.Hurt);
            }
        }
        ifCanHurt += Time.deltaTime;
    }
    private void SwitchAnim()
    {
        anim.SetBool("idle",isIdling);
        anim.SetBool("walk",isWalking);
        anim.SetBool("dead",isDead);
        anim.SetBool("hurt", isHurt);
    }
    protected override void SwitchState(BossState currentState, BossState nextState)
    {
        base.SwitchState(currentState, nextState);
        if (currentState != BossState.Hurt)
        {
            preState = currentState;
        }
        bossState = nextState;
    }

    protected override void EnterIdleState()
    {
        base.EnterIdleState();
        rb.velocity = Vector2.zero;
        isIdling = true;
    }

    protected override void UpdateIdleState()
    {
        base.UpdateIdleState();
        idleTimer += Time.deltaTime;
        rb.velocity = Vector2.zero;
        if (idleTimer > idleTimerMax)
        {
            SwitchState(bossState, BossState.Walk);
        }
    }

    protected override void ExitIdleState()
    {
        base.ExitIdleState();
        isIdling = false;
        idleTimer = 0f;
    }

    protected override void EnterWalkState()
    {
        base.EnterWalkState();
        isWalking = true;
    }

    protected override void UpdateWalkState()
    {
        base.UpdateWalkState();
        walkTimer += Time.deltaTime;
        if (walkTimer >= walkTimerMax)
        {
            SwitchState(bossState, BossState.Idle);
        }
        SetSpeed(walkSpeed);
    }

    protected override void ExitWalkState()
    {
        base.ExitWalkState();
        isWalking = false;
        walkTimer = 0f;
    }

    protected override void EnterDeadState()
    {
        base.EnterDeadState();
        isDead = true;
    }

    protected override void EnterHurtState()
    {
        base.EnterHurtState();
        isHurt = true;
        enemyHealth.isHurt = false;
    }

    protected override void UpdateHurtState()
    {
        base.UpdateHurtState();
        hurtTimer += Time.deltaTime;
        if (hurtTimer > hurtTimerMax)
        {
            SwitchState(bossState, preState);
        }
        rb.velocity = Vector2.zero;
    }

    protected override void ExitHurtState()
    {
        base.ExitHurtState();
        isHurt = false;
        hurtTimer = 0f;
    }
    private void DestroySelf()
    {
        enemyDrops.DropManaClump();
        Destroy(gameObject);
    }
    private void SetSpeed(float speed)
    {
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
            rb.velocity = direction.normalized * speed;
        }
    }
}
