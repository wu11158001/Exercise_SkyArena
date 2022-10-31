using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �귽�޲z����
/// </summary>
public class AssetManagement : MonoBehaviour
{
    static AssetManagement assetManagement;
    public static AssetManagement Instance => assetManagement;

    [Header("����")]
    [Tooltip("�D������")] public GameObject playerObject;
    [Tooltip("�ĤH�h�L����")] public GameObject[] enemySoldierObjects;
    [Tooltip("Boss����")] public GameObject[] bossObjects;
    [Tooltip("�����Ʀr����")] public GameObject hitTextObject;
    [Tooltip("�S�Ī���")] public GameObject[] effectObjects;

    private void Awake()
    {
        if(assetManagement != null)
        {
            Destroy(this);
            return;
        }
        assetManagement = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {        
        OnLoadinSingleAsset(loadPath:"prefab/player", objName:"Player", obj: out playerObject);//�[���D������
        OnLoadinGroupAsset(loadPath: "prefab/enemysoldiers", obj: out enemySoldierObjects);//�[���ĤH�h�L����
        OnLoadinGroupAsset(loadPath: "prefab/boss", obj: out bossObjects);//�[��Boss����
        OnLoadinSingleAsset(loadPath: "prefab/hit_text", objName: "Hit_Text", obj: out hitTextObject);//�[�������Ʀr����
        OnLoadinGroupAsset(loadPath: "prefab/effect", obj: out effectObjects);//�[���S�Ī���
    }    

    /// <summary>
    /// �[����W�귽
    /// </summary>
    /// <param name="loadPath">�[�����|</param>
    /// <param name="objName">�[���W��</param>
    /// <param name="obj">����</param>
    void OnLoadinSingleAsset(string loadPath, string objName, out GameObject obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAsset(objName) as GameObject;
    }

    /// <summary>
    /// �[���h�Ӹ귽
    /// </summary>
    /// <param name="loadPath">�[�����|</param>
    /// <param name="obj">����</param>
    void OnLoadinGroupAsset(string loadPath, out GameObject[] obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAllAssets<GameObject>() as GameObject[];        
    }
}
