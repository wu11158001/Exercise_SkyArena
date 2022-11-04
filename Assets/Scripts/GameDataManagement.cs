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
        
    [Tooltip("PlayerGrade")] public int playerGrade;
    [Tooltip("PlayerExperience")] public int playerExperience;
    [Tooltip("GameLevel")] public int gameLevel;

    [Header("Plugin")]
    [Tooltip("SelectLevel")] public int selectLevel;////using for plugin

    private void Awake()
    {
        if(gameDataManagement != null)
        {
            Destroy(this);
            return;
        }
        gameDataManagement = this;
        DontDestroyOnLoad(gameObject);

        selectLevel = -1;//using for plugin

        OnLoadJsonData();
    }

    /// <summary>
    /// SaveJsonData
    /// </summary>
    public void OnSaveJsonData()
    {
        //Set Value
        PlayerData playerData = new (){ grade = playerGrade, 
                                        experience = playerExperience,
                                        gameLevel = gameLevel};

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
            if (!new FileInfo(filePath).Exists) return;

            StreamReader sr = new StreamReader(filePath);
            string jsonStaring = sr.ReadToEnd();
            sr.Close();

            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonStaring);

            //Set Value
            playerGrade = playerData.grade;
            playerExperience = playerData.experience;
            gameLevel = playerData.gameLevel;
        }
        catch(Exception ex)
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
}
