using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class Weapons_SO : ScriptableObject
{
    public string weaponName;       //武器名字
    public Sprite weaponImage;      //武器的精灵
    public Sprite bulletImage;      //子弹的精灵
    
    public float weaponFireRate;    //子弹发射间隔
    public float weaponDamage;      //武器伤害

    public float weaponBulletSpeed;     //子弹飞行速度
    public float weaponBulletSpread;    //子弹发射范围
    public int weaponBulletAmount;      //每次发射子弹数量
    public float manaAmount;            //每次发射子弹的耗蓝量
    public float blastRadius;
    public AudioClip ShootAudioClip;

}
