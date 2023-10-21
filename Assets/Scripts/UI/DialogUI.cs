using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField]private TextAsset file;
    private List<string> textList = new List<string>();
    private PlayerController playerController;
    //UI组件
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField] private GameObject selectUI;
    private float readSpeed;
    [Header("Buff")] 
    [SerializeField] private GameObject fireUp;
    [SerializeField] private GameObject healthUp;
    
    private bool lineReadFinish;
    private bool interruptRead;
    private int index;          //行数
    private void Awake()
    {
        ReadText(file);
        selectUI.SetActive(false);
    }

    private void OnEnable()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        playerController.ifCanMove = false;
        lineReadFinish = true;//刚开始时出现第一段文字
        StartCoroutine(SetDialogUI());
    }
    private void Update()
    {
        if (index == textList.Count && Input.GetKey(KeyCode.Space))
        {
            //关闭对话框
            playerController.ifCanMove = true;
            index = 0; gameObject.SetActive(false);
        }
        else if(Input.GetKey(KeyCode.Space) && lineReadFinish && !interruptRead)
        {
            StartCoroutine(SetDialogUI());
        }
    }
    private void ReadText(TextAsset file)
    {
        textList.Clear();//读取前将其清空
        index = 0;
        var data = file.text.Split('\n');
        foreach (var line in data)
        {
            textList.Add(line);
        }
    }
    IEnumerator SetDialogUI()
    {
        lineReadFinish = false;
        text.text = "";
        if (textList[index] == "A")
        {
            index++;
        }
        if (textList[index] == "B")
        {
            index++;
            for (int i = 0; i < textList[index].Length; i++)
            {
                text.text += textList[index][i];//按照顺序显示数字
                if (Input.GetKey(KeyCode.Space) && lineReadFinish == false)
                {
                    readSpeed = 0.05f;
                }
                else
                {
                    readSpeed = 0.15f;
                }
                yield return new WaitForSeconds(readSpeed);
            }
            selectUI.SetActive(true);
            interruptRead = true;
            index++;
            lineReadFinish = true;
        }
        else
        {
            for (int i = 0; i < textList[index].Length; i++)
            {
                text.text += textList[index][i];//按照顺序显示数字
                if (Input.GetKey(KeyCode.Space) && lineReadFinish == false)
                {
                    readSpeed = 0.05f;
                }
                else
                {
                    readSpeed = 0.15f;
                }
                yield return new WaitForSeconds(readSpeed);
            }
            index++;
            lineReadFinish = true;            
        }
    }

    public void AddFireUpBuff()
    {
        GameObject spawnPassive = Instantiate(fireUp, transform.position, Quaternion.identity);
        spawnPassive.transform.parent = GameManager.Instance.player;
        interruptRead = false;
        selectUI.SetActive(false);
        StartCoroutine(SetDialogUI());
    }   
    public void AddHealthUpBuff()
    {
        GameObject spawnPassive = Instantiate(healthUp, transform.position, Quaternion.identity);
        spawnPassive.transform.parent = GameManager.Instance.player;
        interruptRead = false;
        selectUI.SetActive(false);
        StartCoroutine(SetDialogUI());
    }
}
