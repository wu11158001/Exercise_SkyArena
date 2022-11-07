using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;

    [Header("Boss")]
    [SerializeField] [Tooltip("SelectBossType")] public int selectBossType;
    [SerializeField] [Tooltip("SelectBossNumber")] public int selectBossNumber;

    [Header("GameData")]
    [Tooltip("PlayerGrade")] public int playerGrade;
    [Tooltip("PlayerExperience")] public int playerExperience;
    [Tooltip("PlayerGold")] public int playerGold;
    [Tooltip("GameLevel")] public int gameLevel;    

    [Header("AFK")]
    [Tooltip("SrartTime")] public DateTime afkRewardStartTime;
    [Tooltip("AFKExperienceReward")] public int afkExperienceReward;
    [Tooltip("AFKGoldReward")] public int afkGoldReward;

    [Header("Skill")]
    [Tooltip("PlayerEquipSkillNumber")] public int[] playEquipSkillNumber;
    [Tooltip("AllSkillGrade")] public int[] allSkillGrade;    
    [Tooltip("PlayerEquipSkills")]PlayerSkillBehavior[] playerEquipSkills_Array = 
        new PlayerSkillBehavior[] { new PlayerSkillBehavior(), new PlayerSkillBehavior() , new PlayerSkillBehavior() , new PlayerSkillBehavior() };

    private void Awake()
    {
        if (gameDataManagement != null)
        {
            Destroy(this);
            return;
        }
        gameDataManagement = this;
        DontDestroyOnLoad(gameObject);

        //Plugin
        selectBossType = -1;//using for plugin

        //GameData
        playerGrade = 1;
        gameLevel = 1;

        OnLoadJsonData();
    }

    private void Update()
    {
        OnPlayerImplementSkill();
    }

    /// <summary>
    /// PlayerImplementSkill
    /// </summary>
    void OnPlayerImplementSkill()
    {
        if(GameManagement.Instance != null && GameManagement.Instance.GetPlayerObject != null)
        {
            for (int i = 0; i < playerEquipSkills_Array.Length; i++)
            {
                if (playEquipSkillNumber[i] >= 0)
                {
                    playerEquipSkills_Array[i].OnSkillCD(skillNumber: playEquipSkillNumber[i],
                                                         cd: NumericalValueManagement.NumbericalValue_PlayerSkill.playerSkillsCD[i]);
                }
            }
        }
    }

    /// <summary>
    /// CalculateSkillValues
    /// </summary>
    /// <param name="skillNumber"></param>
    public int OnCalculateSkillValues(int skillNumber)
    {
        int value = 0;
        int grade = allSkillGrade[skillNumber] - 1 <= 0 ? 0 : allSkillGrade[skillNumber] - 1;
        value = NumericalValueManagement.NumbericalValue_PlayerSkill.initialSkillValue[skillNumber] +
                       (grade * NumericalValueManagement.NumbericalValue_PlayerSkill.raiseSkillValue[skillNumber]);

        return value;
    }

    /// <summary>
    /// AFKRewardTime
    /// </summary>
    public TimeSpan OnAFKRewardTime()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSpan = currentTime.Subtract(afkRewardStartTime);
        return timeSpan;
    }
    
    /// <summary>
    /// SaveJsonData
    /// </summary>
    public void OnSaveJsonData()
    {
        //Set Value
        PlayerData playerData = new()
        {
            grade = playerGrade,
            experience = playerExperience,
            gold = playerGold,
            gameLevel = gameLevel,
            afkRewardStartTime = DateTime.Now.ToString(),
            allSkillGrade = allSkillGrade,
            equipSkillNumber = playEquipSkillNumber
        };

        //Create Or Open File
        string filePath = Application.dataPath + "/JsonSaveFile";
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        FileInfo fileInfo = new FileInfo(filePath + "/JsonSave.json");
        if (fileInfo.Exists) fileInfo.Delete();
        StreamWriter sw = fileInfo.CreateText();

        //Write Data
        string saveJsonString = JsonUtility.ToJson(playerData, true);
        sw.Write(saveJsonString);
        sw.Close();
    }

    /// <summary>
    /// LoadJsonData
    /// </summary>
    public void OnLoadJsonData()
    {
        string filePath = Application.dataPath + "/JsonSaveFile/JsonSave.json";

        try
        {
            if (!new FileInfo(filePath).Exists)
            {
                afkRewardStartTime = DateTime.Now;
                allSkillGrade = new int[] { 0, 0, 0, 0 };
                playEquipSkillNumber = new int[] { -1, -1, -1, -1 };
                return;
            }

            StreamReader sr = new StreamReader(filePath);
            string jsonStaring = sr.ReadToEnd();
            sr.Close();

            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonStaring);

            //Set Value
            playerGrade = playerData.grade;
            playerExperience = playerData.experience;
            playerGold = playerData.gold;
            gameLevel = playerData.gameLevel;
            afkRewardStartTime = Convert.ToDateTime(playerData.afkRewardStartTime);
            allSkillGrade = playerData.allSkillGrade;
            playEquipSkillNumber = playerData.equipSkillNumber;
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Fail:" + ex.Message);
        }
    }
}

class PlayerData
{
    public int grade;
    public int experience;
    public int gold;
    public int gameLevel;
    public string afkRewardStartTime;
    public int[] allSkillGrade;
    public int[] equipSkillNumber;
}
