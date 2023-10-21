using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class Boss_Alien : Enemy
{
    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private EnemyDrops enemyDrops;
    private EnemyHealth enemyHealth;
    [SerializeField]
    private BossState bossState,preState;
    private bool isLeft;
    //动画相关
    private bool isIdling;
    private bool isWalking;
    private bool isDead;
    private bool isFiring;
    private bool isAttack;
    private bool isRunning;
    private bool isHurt;
    //设置状态时间
    private float idleTimer;
    private float walkTimer;
    private float runTimer;
    private float attackTimer;
    private float ifAttackTimer;
    private float touchDamageTimer;
    private float hurtTimer;
    private float ifCanHurt;
    private float ifCanFire;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    
    [SerializeField] private float idleTimerMax = 2f;
    [SerializeField] private float runTimerMax = 3f;
    [SerializeField] private float walkTimerMax = 1.5f;   
    [SerializeField] private float hurtTimerMax = 0.45f;   
    [SerializeField] private float attackTimerMax = 2f;
    [SerializeField] private float ifAttackTimerMax = 1f;
    [SerializeField] private float touchDamageTimerMax;
    
    //设置碰撞伤害参数
    [SerializeField] private Transform touchDamageCheck;
    [SerializeField] private float touchDamage;
    [SerializeField] private float touchDamageWidth, touchDamageHeight;
    [SerializeField] private LayerMask whatIsPlayer;
    private Vector2 touchDamageBotLeft;
    private Vector2 touchDamageTopRight;

    //攻击相关
    private Transform laser;
    private Transform bulletSpawner;
    //音效
    [SerializeField]
    private AudioClip[] bossAudio; 
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        enemyDrops = GetComponent<EnemyDrops>();
        enemyHealth = GetComponent<EnemyHealth>();
        laser = transform.GetChild(1);
        laser.gameObject.SetActive(false);
        bulletSpawner = transform.GetChild(2);
        bulletSpawner.gameObject.SetActive(false);
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
            case BossState.Run:
                UpdateRunState();
                break;
            case BossState.Attack:
                UpdateAttackState();
                break;
            case BossState.Fire:
                UpdateFireState();
                break;          
            case BossState.Hurt:
                UpdateHurtState();
                break;
        }
        SwitchAnim();
        TakeTouchDamage();
        if (enemyHealth.currentHealth <= 0)
        {
            SwitchState(bossState, BossState.Dead);
        }

        if (enemyHealth.isHurt)
        {
            if (bossState != BossState.Attack && bossState != BossState.Fire)
            {
                if (ifCanHurt > 5f)
                {
                    ifCanHurt = 0f;
                    SwitchState(bossState, BossState.Hurt);                    
                }

                if (ifCanFire > 3f)
                {
                    ifCanFire = 0f;
                    SwitchState(bossState, BossState.Fire);
                    isHurt = false;
                }
            }
        }

        if (ifAttackTimer >= ifAttackTimerMax && bossState!= BossState.Fire)
        {
            SwitchState(bossState,BossState.Attack);
        }
        ifCanFire += Time.deltaTime;
        ifCanHurt += Time.deltaTime;
    }

    private void SwitchAnim()
    {
        anim.SetBool("idle",isIdling);
        anim.SetBool("walk",isWalking);
        anim.SetBool("dead",isDead);
        anim.SetBool("firing",isFiring);
        anim.SetBool("attack",isAttack);
        anim.SetBool("run", isRunning);
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
//----Idle State-----
    protected override void EnterIdleState()
    {
        base.EnterIdleState();
        //速度设为0
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
//-----Walk State-----
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
            SwitchState(bossState, BossState.Run);
        }
        SetSpeed(walkSpeed);
    }

    protected override void ExitWalkState()
    {
        base.ExitWalkState();
        isWalking = false;
        walkTimer = 0f;
    }
//-----Run To Attack Player State,同时造成碰撞伤害-----
    protected override void EnterRunState()
    {
        base.EnterRunState();
        isRunning = true;
    }

    protected override void UpdateRunState()
    {
        base.UpdateRunState();
        runTimer += Time.deltaTime;
        if (runTimer >= runTimerMax)
        {
            SwitchState(bossState, BossState.Idle);
        }
        SetSpeed(runSpeed);
    }

    protected override void ExitRunState()
    {
        base.ExitRunState();
        isRunning = false;
        runTimer = 0f;
    }
//-----Attack State-----
    protected override void EnterAttackState()
    {
        base.EnterAttackState();
        isAttack = true;
        audioSource.clip = bossAudio[0];
        audioSource.Play();
    }

    protected override void UpdateAttackState()
    {
        base.UpdateAttackState();
        attackTimer += Time.deltaTime;
        rb.velocity = Vector2.zero;

        if (!laser.gameObject.activeSelf && attackTimer >= attackTimerMax)
        {
            SwitchState(bossState, BossState.Fire);
        }
    }

    protected override void ExitAttackState()
    {
        base.ExitAttackState();
        isAttack = false;
        attackTimer = 0f;
        ifAttackTimer = 0f;
    }
//-----Fire State-----
    protected override void EnterFireState()
    {
        base.EnterFireState();
        isFiring = true;
    }

    protected override void UpdateFireState()
    {
        rb.velocity = Vector2.zero;
        base.UpdateFireState();
    }

    protected override void ExitFireState()
    {
        isFiring = false;
        bulletSpawner.gameObject.SetActive(false);
        base.ExitFireState();
    }

    //-----Dead State-----
    protected override void EnterDeadState()
    {
        base.EnterDeadState();
        isDead = true;
    }
//-----Hurt State-----
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
            if (preState == BossState.Attack || preState == BossState.Fire)
            {
                preState = BossState.Idle;
            }
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
    //用于动画中调用的事件函数
    private void DestroySelf()
    {
        enemyDrops.DropChest();
        enemyDrops.DropManaClump();
        Destroy(gameObject);
    }

    private void EmitLaser()
    {
        laser.gameObject.SetActive(true);
    }

    private void EmitBullet()
    {
        bulletSpawner.gameObject.SetActive(true);
    }

    private void SwitchToIdleState()
    {
        SwitchState(bossState, BossState.Idle);
    }
    
    private void TakeTouchDamage()      //对玩家造成碰撞伤害
    {        
        touchDamageTimer += Time.deltaTime;
        if (touchDamageTimer > touchDamageTimerMax)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2),
                touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2),
                touchDamageCheck.position.y + (touchDamageHeight / 2));
            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);//设置检测区域
            if (hit)
            {
                hit.GetComponent<PlayerController>().TakeDamage(touchDamage);
                touchDamageTimer = 0f;
            }
        }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //设置检测接触位置的四个位置
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2),
            touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2),
            touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2),
            touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2),
            touchDamageCheck.position.y + (touchDamageHeight / 2));
        //画出对应位置的线
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ifAttackTimer += Time.deltaTime;
        }
    }
}
