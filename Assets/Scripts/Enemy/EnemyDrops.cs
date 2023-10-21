using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] private GameObject manaClump;      //产生的蓝量块
    [SerializeField] private GameObject bloodClump;
    [SerializeField] private float manaAmount;          //每个蓝量块包含的蓝量大小
    [SerializeField] private float bloodAmount;          //每个血量块包含的蓝量大小
    
    [SerializeField] private GameObject chest;
    public void DropManaClump()
    {
        int rand = Random.Range(0, 10);
        if (rand < 5)
        {
            GameObject manaDroped = Instantiate(manaClump, transform.position, Quaternion.identity);
            manaDroped.GetComponent<ManaClump>().heldMana = manaAmount;
        }else if (rand < 7)
        {
            GameObject bloodDroped = Instantiate(bloodClump, transform.position, Quaternion.identity);
            bloodDroped.GetComponent<BloodClump>().heldBlood = bloodAmount;
        }
    }

    public void DropChest()
    {
        Instantiate(chest, transform.position, Quaternion.identity);
    }
}
