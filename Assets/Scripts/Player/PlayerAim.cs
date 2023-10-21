using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public float rotateSpeed = 15f;     //设置旋转速度
    private float angle;
    
    private void Update()
    {//Camera.main.ScreenToWorldPoint(Input.mousePosition)将鼠标的位置从屏幕空间转换到世界空间
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        //Mathf.Atan2(float y, float x)返回tan y/x,以弧度为单位，Mathf.Rad2Deg用于将弧度换算为常量
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //进行球形插值设置旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    public float GetAngle()
    {
        return angle;
    }
}
