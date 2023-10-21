using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance;

    [SerializeField] private float speed;
     private Transform followTarget;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (followTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z),
                speed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        followTarget = newTarget;
    }
}
