using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遊戲資料中心
/// </summary>
public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;

    [Header("玩家")]
    [Tooltip("玩家等級")] public int playerLevel;
    [Tooltip("玩家經驗值")] public int playerExperience;

    [Header("關卡")]
    [Tooltip("關卡等級")] public int gameLevel;

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
