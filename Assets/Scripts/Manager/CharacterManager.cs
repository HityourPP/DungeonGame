using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterInfo_SO[] characterInfoSo;
    private int selectCharacterID;

    public Image[] imageObjects;
    public TextMeshProUGUI[] textObjects;
    private void Start()
    {
        SetSelectCharacter();
    }

    public void NextCharacter()
    {
        if (selectCharacterID + 1 == characterInfoSo.Length)
        {
            selectCharacterID = 0;
        }
        else
        {
            selectCharacterID++;
        }
        SetSelectCharacter();
    }

    public void PreCharacter()
    {
        if (selectCharacterID - 1 < 0)
        {
            selectCharacterID = characterInfoSo.Length - 1;
        }
        else
        {
            selectCharacterID--;
        }
        SetSelectCharacter();
    }

    private void SetSelectCharacter()
    {
        CharacterHolder.Instance.selectCharacter = characterInfoSo[selectCharacterID];
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < textObjects.Length; i++)    //显示对应文本
        {
            textObjects[i].text = characterInfoSo[selectCharacterID].characterInfo[i];
        }

        for (int i = 0; i < imageObjects.Length; i++)   //显示对应图片
        {
            imageObjects[i].sprite = characterInfoSo[selectCharacterID].sprites[i];
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
