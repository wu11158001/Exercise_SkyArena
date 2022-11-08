using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using UnityEngine.Networking;

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

        OnScreenOrientation();

        //Plugin
        selectBossType = -1;//using for plugin

        //GameData
        playerGrade = 1;
        gameLevel = 1;
        afkRewardStartTime = DateTime.Now;
        allSkillGrade = new int[] { 0, 0, 0, 0, 0 };
        playEquipSkillNumber = new int[] { -1, -1, -1, -1 };

        OnLoadJsonData();

#if UNITY_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
    }

    /// <summary>
    /// ScreenOrientation
    /// </summary>
    void OnScreenOrientation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
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
        if(GameManagement.Instance != null && 
           GameManagement.Instance.GetPlayerObject != null &&
           GameManagement.Instance.GetPlayerObject.Hp > 0)
        {
            for (int i = 0; i < playerEquipSkills_Array.Length; i++)
            {
                if (playEquipSkillNumber[i] >= 0)
                {
                    playerEquipSkills_Array[i].OnSkillCD(equipNumber: i,
                                                         skillNumber: playEquipSkillNumber[i],
                                                         cd: NumericalValueManagement.NumbericalValue_PlayerSkill.playerSkillsCD[playEquipSkillNumber[i]]);
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

        FileInfo fileInfo = null;
#if UNITY_EDITOR_WIN
        string filePath = Application.streamingAssetsPath + "/JsonSaveFile";
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        fileInfo = new FileInfo(filePath + "/JsonData.json");       
#elif UNITY_ANDROID
        fileInfo = new FileInfo(Application.persistentDataPath + "/JsonData.json");        
#endif
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
        StreamReader sr = null;
        string filePath = "";
#if UNITY_EDITOR_WIN
        filePath = Application.streamingAssetsPath + "/JsonSaveFile";
#elif PLATFORM_ANDROID
        filePath = filePath = Application.persistentDataPath + "/JsonData.json";
#endif
        try
        {
            if (!new FileInfo(filePath).Exists)
            {
                return;
            }

            sr = new StreamReader(filePath);
            string jsonStaring = sr.ReadToEnd();
            sr.Close();

            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonStaring);

            //Set Value
            playerGrade = playerData.grade;
            playerExperience = playerData.experience;
            playerGold = playerData.gold;
            gameLevel = playerData.gameLevel;
            afkRewardStartTime = Convert.ToDateTime(playerData.afkRewardStartTime);
            playEquipSkillNumber = playerData.equipSkillNumber;

            if (allSkillGrade.Length != playerData.allSkillGrade.Length)
            {
                for (int i = 0; i < playerData.allSkillGrade.Length; i++)
                {
                    allSkillGrade[i] = playerData.allSkillGrade[i];
                }
            }
            else allSkillGrade = playerData.allSkillGrade;
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
