using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClip_SO audioClipSo;
    [SerializeField] private float volume;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }
}
