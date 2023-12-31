using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    private GameObject mapSprite;

    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(2).gameObject;
        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
        }
    }
}
