using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject tipUI;
    [SerializeField] private GameObject[] weaponGameObject; //宝箱中的物品
    [SerializeField] private GameObject manaGameObject;
    private Animator anim;
    private bool isPlayerAround;
    private bool ifIsOpen;
    public bool isWeapon;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        InputManager.Instance.OnInteraction += InputManagerOnInteraction;
        isPlayerAround = false;
        ifIsOpen = false;
        tipUI.SetActive(false);
    }

    private void InputManagerOnInteraction(object sender, EventArgs e)
    {
        if (isPlayerAround)
        {
            anim.SetBool("open", true);
            ifIsOpen = true;
            tipUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnInteraction -= InputManagerOnInteraction;
    }

    public void OpenChest()
    {
        //TODO:生成宝箱内的物品   
        if (isWeapon)
        {
            Instantiate(weaponGameObject[Random.Range(0, weaponGameObject.Length)], transform.position,
                Quaternion.identity);
        }
        else
        {
            if (Random.Range(0, 100) > 10)
            {//生成蓝量
                for (int i = 0; i < 10; i++)
                {
                    GameObject mana = Instantiate(manaGameObject, new Vector3(transform.position.x + Random.Range(-2f, 2f),
                        transform.position.y + Random.Range(-2f, 2f), transform.position.z), Quaternion.identity);
                    mana.GetComponent<ManaClump>().heldMana = 10f;
                }
            }
            else
            {
                Instantiate(weaponGameObject[Random.Range(0, weaponGameObject.Length)], transform.position,
                    Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerAround = true;
            if(!ifIsOpen) tipUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tipUI.SetActive(false);
            isPlayerAround = false;
        }
    }
}
