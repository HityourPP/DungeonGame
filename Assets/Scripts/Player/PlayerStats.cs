using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float moveSpeedStat;
    public float maxHealth;
    public float maxMana;           //最大蓝量
    
    public Sprite weaponImage;      //武器的精灵
    public Sprite bulletImage;      //子弹的精灵
    
    //下面设置的为角色自身的增益倍率
    public float fireRate;      //子弹发射间隔
    public float damage;        //武器伤害
    public float bulletSpeed;   //子弹飞行速度
    public float bulletLifeTime;
    //下面两个是实际的增加大小
    public float bulletSpread;  //子弹发射范围
    public int bulletAmount;    //每次发射子弹数量

    public float armorStat;         //角色护甲减伤倍率
    public float blastRadiusStat;   //子弹爆炸范围
}
