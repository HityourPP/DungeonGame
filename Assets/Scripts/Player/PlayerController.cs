using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public event EventHandler<PlayerFillAmountNormalize> OnPlayerHealthBar;
    public event EventHandler<PlayerFillAmountNormalize> OnPlayerManaBar;
    public bool ifCanMove;
    public class PlayerFillAmountNormalize: EventArgs
    {
        public float fillAmountNormalize;
    }

    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform weapon;
    //近战攻击
    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    private bool isMeleeAttacking;
    private CharacterInfo_SO selectCharacter;
    public Weapons_SO weaponData;
    
    private Transform hand;
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    private float speed;
    
    private Vector3 direction;
    private bool isWalking;
    private bool isLeft;
    private bool isFiring;

    private bool ifCanMelee;
    public bool isAbilityReleasing;

    private SpriteRenderer weaponImage;
    private SpriteRenderer bulletImage;

    private float currentHealth;
    private float currentMana;
    private float fireTimer;
    public float startTimer;
    //音效
    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip healClip;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hand = transform.GetChild(0).transform;
        
        playerStats = GetComponent<PlayerStats>();
        weaponImage = weapon.GetComponent<SpriteRenderer>();
        bulletImage = bulletPrefab.GetComponent<SpriteRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
        ifCanMove = true;
        ifCanMelee = true;
    }

    private void Start()
    {
        selectCharacter = CharacterHolder.Instance.selectCharacter;
        InputManager.Instance.OnAbilityRelease += InputManagerOnAbilityRelease;
        InputManager.Instance.OnMeleeAttack += InputManagerOnMeleeAttack;
        RefreshPlayerStats();
    }

    private void InputManagerOnMeleeAttack(object sender, EventArgs e)
    {
        if (!isFiring && ifCanMelee)
        {
            PlayerMeleeAttack();
        }
    }

    private void InputManagerOnAbilityRelease(object sender, EventArgs e)
    {
        if (Time.time > startTimer + selectCharacter.coolDown)
        {
            GetComponentInChildren<PlayerAbility>().ActivateAbility();
            startTimer = Time.time;
        }
    }

    private void Update()
    {
        if (ifCanMove)
        {
            MoveController();
            if (Input.GetMouseButton(0))
            {
                isFiring = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isFiring = false;
            }
            if (isFiring)
            {
                if (fireTimer > 1 / weaponData.weaponFireRate * Mathf.Abs(1 - playerStats.fireRate))  //设置攻击间隔
                {
                    fireTimer = 0f;
                    if (currentMana > 0 && !isMeleeAttacking)
                    {
                        PlayerShootAttack();   //蓝量足够时，产生子弹  
                    }
                }
            }            
            fireTimer += Time.deltaTime;
        }
        AnimationController();
    }
    private void PlayerShootAttack()
    {
        weaponImage.sprite = weaponData.weaponImage;
        bulletImage.sprite = weaponData.bulletImage;
        for (int i = 0; i < weaponData.weaponBulletAmount + playerStats.bulletAmount; i++) //设置每次子弹的生成数目
        {

            //设置射击音效
            playerAudioSource.clip = weaponData.ShootAudioClip;
            playerAudioSource.Play();
                
            Transform spawnBullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
            Bullet bulletScript = spawnBullet.GetComponent<Bullet>();

            bulletScript.damage = weaponData.weaponDamage * (1 + playerStats.damage);                      //设置子弹伤害
            bulletScript.lifeTime = playerStats.bulletLifeTime * (1 + playerStats.bulletLifeTime);         //设置子弹生存时间
            bulletScript.bulletSpeed = weaponData.weaponBulletSpeed * (1 + playerStats.bulletSpeed);       //设置子弹的飞行速度
            bulletScript.blastRadius = weaponData.blastRadius * (1 + playerStats.blastRadiusStat);         //设置子弹的爆炸半径
            //设置子弹的攻击范围
            spawnBullet.Rotate(0, 0,
                Random.Range(-weaponData.weaponBulletSpread - playerStats.bulletSpread,
                    weaponData.weaponBulletSpread + playerStats.bulletSpread));
            if (!isAbilityReleasing)    //释放技能时不消耗蓝量
            {
                    currentMana -= weaponData.manaAmount;       //设置其蓝量减少                    
            }
            OnPlayerManaBar?.Invoke(this,new PlayerFillAmountNormalize()
            {
                   fillAmountNormalize =  currentMana/playerStats.maxMana
            });
        }
    }

    private void PlayerMeleeAttack()
    {
        // ifCanMelee = false;
        isMeleeAttacking = true;
        anim.SetTrigger("attack");
        Collider2D[] detectedObjects =
            Physics2D.OverlapCircleAll(meleeAttackPosition.position, meleeAttackRadius, whatIsEnemy);
        foreach (var col in detectedObjects)
        {
            col.GetComponent<EnemyHealth>().TakeDamage(selectCharacter.meleeAttackDamage);
        }
    }
    private void OnDrawGizmos()//画出攻击范围
    {
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackRadius);
    }
    private void MoveController()
    {
        Vector2 moveDir = InputManager.Instance.GetMoveDir();
        isWalking = moveDir != Vector2.zero;
        rb.velocity = new Vector2(moveDir.x * speed, moveDir.y * speed);
        // rb.position += moveDir * speed * Time.deltaTime;
        //转向设置
        if ((playerAim.GetAngle() > 90f && playerAim.GetAngle() < 180f) || (playerAim.GetAngle() > -180f && playerAim.GetAngle() < -90f))
        {
            if (!isLeft)
            {
                isLeft = true;
                transform.Rotate(0, 180f, 0);
                        hand.transform.localScale = new Vector3(1f, -1f, 1f);
            }
        }
        if ((playerAim.GetAngle() > -90f && playerAim.GetAngle() < 90f))
        {
            if (isLeft)
            {
                isLeft = false;
                transform.Rotate(0, 180f, 0);      
                hand.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void AnimationController()
    {
        anim.SetBool("isWalking", isWalking);
    }
    
    private void RefreshPlayerStats()//更新玩家状态
    {
        speed = playerStats.moveSpeedStat;
        currentHealth = playerStats.maxHealth;
        currentMana = playerStats.maxMana;
        RefreshUI();
    }

    public void RefreshUI()
    {
        OnPlayerHealthBar?.Invoke(this,new PlayerFillAmountNormalize()
        {
            fillAmountNormalize =  currentHealth/playerStats.maxHealth
        });        
        OnPlayerManaBar?.Invoke(this,new PlayerFillAmountNormalize()
        {
            fillAmountNormalize =  currentMana/playerStats.maxMana
        });
    }

    public void RefreshWeaponImage()
    {
        weaponImage.sprite = weaponData.weaponImage;
        bulletImage.sprite = weaponData.bulletImage;
    }
    
    public void TakeDamage(float damage)  //对玩家造成伤害
    {
        anim.SetTrigger("hurt");
        currentHealth -= damage * (1 - playerStats.armorStat);          //设置护甲减伤倍率
        OnPlayerHealthBar?.Invoke(this,new PlayerFillAmountNormalize
        {
            fillAmountNormalize =  currentHealth/playerStats.maxHealth
        });
        if (currentHealth <= 0)
        {
            GameManager.Instance.ifPlayerAlive = false;
            Destroy(gameObject);
        }
    }

    public void GainMana(float manaAmount)
    {
        playerAudioSource.clip = healClip;
        playerAudioSource.Play();
        currentMana = Mathf.Min(currentMana + manaAmount, playerStats.maxMana);
        OnPlayerManaBar?.Invoke(this,new PlayerFillAmountNormalize()
        {
            fillAmountNormalize =  currentMana/playerStats.maxMana
        });
        
    }
    public void GainHealth(float healthAmount)
    {
        playerAudioSource.clip = healClip;
        playerAudioSource.Play();
        currentHealth = Mathf.Min(currentHealth + healthAmount, playerStats.maxHealth);
        OnPlayerHealthBar?.Invoke(this,new PlayerFillAmountNormalize()
        {
            fillAmountNormalize =  currentHealth/playerStats.maxHealth
        });
    }
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = Math.Min(newHealth, playerStats.maxHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetMeleeAttackOver()
    {
        isMeleeAttacking = false;
    }
}
