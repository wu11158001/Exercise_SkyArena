using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;
        
    [Tooltip("PlayerLevel")] public int playerLevel;
    [Tooltip("PlayerExperience")] public int playerExperience;
    [Tooltip("GameLevel")] public int gameLevel;
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
    }
}
