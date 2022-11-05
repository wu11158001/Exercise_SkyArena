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

    [Header("Plugin")]
    [SerializeField] [Tooltip("SelectLevel")] public int selectLevel;////using for plugin

    [Header("GameData")]
    [Tooltip("PlayerGrade")] public int playerGrade;
    [Tooltip("PlayerExperience")] public int playerExperience;
    [Tooltip("GameLevel")] public int gameLevel;

    [Header("AFK")]
    [Tooltip("SrartTime")] public DateTime afkRewardStartTime;

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
        selectLevel = -1;//using for plugin

        //GameData
        playerGrade = 1;
        gameLevel = 1;

        OnLoadJsonData();
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
            gameLevel = gameLevel,
            afkRewardStartTime = DateTime.Now.ToString()
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
                return;
            }

            StreamReader sr = new StreamReader(filePath);
            string jsonStaring = sr.ReadToEnd();
            sr.Close();

            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonStaring);

            //Set Value
            playerGrade = playerData.grade;
            playerExperience = playerData.experience;
            gameLevel = playerData.gameLevel;
            afkRewardStartTime = Convert.ToDateTime(playerData.afkRewardStartTime);
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
    public int gameLevel;
    public string afkRewardStartTime;
}
