using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Room : MonoBehaviour
{
    public GameObject up, down, left, right;
    public bool isUp, isDown, isLeft, isRight;
    public int doorAmount;
    public int stepToStart;
    public int roomNum; //生成的房间编号
    
    [SerializeField] private TextMeshProUGUI num;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private GameObject weaponChest;
    [SerializeField] private GameObject manaChest;
    [SerializeField] private Transform bossPrefabs;
    //NPC
    [SerializeField] private GameObject NPC;
    [SerializeField] private Transform NPCPos;
    private Transform boss;
    
    private Animator animUp, animDown, animLeft, animRight;
    private bool hasChest;

    private void Awake()
    {
        animUp = up.GetComponent<Animator>();
        animDown = down.GetComponent<Animator>();
        animLeft = left.GetComponent<Animator>();
        animRight = right.GetComponent<Animator>();
    }

    private void Start()
    {
        up.SetActive(isUp);
        down.SetActive(isDown);
        left.SetActive(isLeft);
        right.SetActive(isRight);
        enemySpawner.SetActive(false);
        OpenDoor();
        if (roomNum == -2|| roomNum == 1)   //设置宝箱房间
        {   //生成一个武器宝箱
            Instantiate(weaponChest, transform.position, Quaternion.identity);
        }
        else if (roomNum == -1)  //TODO:设置最终房间
        {
            boss = Instantiate(bossPrefabs, transform.position, Quaternion.identity);
            boss.gameObject.SetActive(false);
        }
        else if(roomNum == -3)   //TODO:设置起始房间
        {
            Instantiate(NPC, NPCPos);
            // boss = Instantiate(bossPrefabs, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y),
            new Vector2(16f, 10f), 360f, enemyLayerMask);
        if ( roomNum > 1)   //设置敌人房间
        {
            // Debug.Log(enemy.Length);
            //TODO:敌人数目为0时，打开门
            if (enemySpawner.activeSelf)
            {
                if (enemy.Length <= 0 && enemySpawner.GetComponent<EnemySpawner>().IfIsOver())
                { //敌人数目为0，并且敌人生成完毕后，打开门
                    OpenDoor();
                    if (!hasChest)
                    {
                        Instantiate(manaChest, transform.position, Quaternion.identity);//生成一个宝箱
                        hasChest = true;
                    }
                }
            }
        }
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawLine(new Vector3(transform.position.x + 8f, transform.position.y + 4f, 0),
    //         new Vector3(transform.position.x + 8f, transform.position.y - 4f, 0));       
    //     Gizmos.DrawLine(new Vector3(transform.position.x + 8f, transform.position.y + 4f, 0),
    //         new Vector3(transform.position.x - 8f, transform.position.y + 4f, 0));       
    //     Gizmos.DrawLine(new Vector3(transform.position.x - 8f, transform.position.y + 4f, 0),
    //         new Vector3(transform.position.x - 8f, transform.position.y - 4f, 0));      
    //     Gizmos.DrawLine(new Vector3(transform.position.x - 8f, transform.position.y - 4f, 0),
    //         new Vector3(transform.position.x + 8f, transform.position.y - 4f, 0));
    // }

    public void UpdateRoom(float x,float y)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / x) + Mathf.Abs(transform.position.y / y));
        num.text = stepToStart.ToString();
        if(isLeft)doorAmount++;
        if(isRight)doorAmount++;
        if(isUp)doorAmount++;
        if(isDown)doorAmount++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))         //当角色进入时
        {
            MoveCamera.Instance.ChangeTarget(transform);
            if (roomNum > 1)
            {
                CloseDoor();    //敌人房间进行关门
                enemySpawner.SetActive(true);
            }
            if (roomNum == -1)
            {
                boss.gameObject.SetActive(true);
            }
        }
    }

    private void OpenDoor()
    {
        animUp.SetBool("open", true);
        animDown.SetBool("open", true);
        animLeft.SetBool("open", true);
        animRight.SetBool("open", true);
    }

    private void CloseDoor()
    {
        animUp.SetBool("open", false);
        animDown.SetBool("open", false);
        animLeft.SetBool("open", false);
        animRight.SetBool("open", false);
    }
}
