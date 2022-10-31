using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C����Ƥ���
/// </summary>
public class GameDataManagement : MonoBehaviour
{
    static GameDataManagement gameDataManagement;
    public static GameDataManagement Instance => gameDataManagement;

    [Header("���a")]
    [Tooltip("���a����")] public int playerLevel;
    [Tooltip("���a�g���")] public int playerExperience;

    [Header("���d")]
    [Tooltip("���d����")] public int gameLevel;

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
