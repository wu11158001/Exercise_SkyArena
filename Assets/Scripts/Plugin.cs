using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Plugin : MonoBehaviour
{
    [SerializeField] [Tooltip("InputField")] TMP_InputField thisInputField;

    private void Start()
    {
        thisInputField = FindChild.OnFindChild<TMP_InputField>(transform, "Plugin_TMPInputField");
        thisInputField.gameObject.SetActive(false);
    }  
    
    void Update()
    {
        OnOpenPlugin();
    }

    /// <summary>
    /// OpenPlugin
    /// </summary>
    void OnOpenPlugin()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            bool isActive = thisInputField.gameObject.activeSelf ? false : true;
            thisInputField.gameObject.SetActive(isActive);
            thisInputField.Select();

            if (thisInputField.text.Length > 0)
            {
                OnJudgeInput();
                thisInputField.text = "";
            }
        }
    }

    /// <summary>
    /// JudgeInput
    /// </summary>
    void OnJudgeInput()
    {
        OnSelectBossType();
        OnBossHp();
        OnGold();
    }

    void OnGold()
    {
        if (thisInputField.text.StartsWith("gold"))
        {
            var split = thisInputField.text.Split('=');
            try
            {
                GameDataManagement.Instance.playerGold = Int32.Parse(split[1]);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    /// <summary>
    /// BossHp
    /// </summary>
    void OnBossHp()
    {
        if (thisInputField.text.StartsWith("skillboss"))
        {           
            try
            {
                GameObject boss = GameManagement.Instance.GetEnemyList[0].gameObject;
                boss.SetActive(false);
                boss.GetComponent<AIPlayer>().OnBossDeath();                
            }
            catch
            {
                Debug.LogError("skillboss fail");
            }
        }
    }

    /// <summary>
    /// SelectBossType
    /// </summary>
    void OnSelectBossType()
    {        
        if (thisInputField.text.StartsWith("bosstype"))
        {
            var split = thisInputField.text.Split('=');

            try
            {
                int selectLevel = Convert.ToInt32(split[1]);
                if (selectLevel > 3)
                {
                    Debug.LogError("maximum 3");
                    return;
                }

                GameDataManagement.Instance.selectLevel = selectLevel;
                Debug.LogError($"selectLevel{selectLevel}");
            }
            catch
            {
                Debug.LogError("input warning");
            }
        }
    }
}
