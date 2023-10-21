using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class CharacterInfo_SO : ScriptableObject
{
    public string[] characterInfo;  //使用数组保存，以便于在unity中进行扩展
    
    public Sprite[] sprites;

    public Weapons_SO weapon;       //携带的武器的数据
    public float coolDown;	        //技能CD
    public GameObject ability;
    public GameObject passive;
    
    public float abilityDuration;   //技能持续时间
    public GameObject playerGameObject;
    public float meleeAttackDamage;
}
