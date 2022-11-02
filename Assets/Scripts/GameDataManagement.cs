using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;
        
    [Tooltip("PlayerLevel")] public int playerLevel;
    [Tooltip("PlayerExperience")] public int playerExperience;
    [Tooltip("gameLevel")] public int gameLevel;

    private void Awake()
    {
        if(gameDataManagement != null)
        {
            Destroy(this);
            return;
        }
        gameDataManagement = this;
        DontDestroyOnLoad(gameObject);
    }
}
