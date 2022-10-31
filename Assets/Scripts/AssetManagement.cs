using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManagement : MonoBehaviour
{
    static AssetManagement assetManagement;
    public static AssetManagement Instance => assetManagement;

    [Header("AssetObjects")]
    [Tooltip("playerObject")] public GameObject playerObject;
    [Tooltip("enemySoldierObjects")] public GameObject[] enemySoldierObjects;
    [Tooltip("bossObjects")] public GameObject[] bossObjects;
    [Tooltip("hitTextObject")] public GameObject hitTextObject;
    [Tooltip("effectObjects")] public GameObject[] effectObjects;

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
        OnLoadinSingleAsset(loadPath:"prefab/player", objName:"Player", obj: out playerObject);
        OnLoadinGroupAsset(loadPath: "prefab/enemysoldiers", obj: out enemySoldierObjects);
        OnLoadinGroupAsset(loadPath: "prefab/boss", obj: out bossObjects);
        OnLoadinSingleAsset(loadPath: "prefab/hit_text", objName: "Hit_Text", obj: out hitTextObject);
        OnLoadinGroupAsset(loadPath: "prefab/effect", obj: out effectObjects);
    }

    /// <summary>
    /// LoadinSingleAsset
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objName"></param>
    /// <param name="obj"></param>
    void OnLoadinSingleAsset(string loadPath, string objName, out GameObject obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAsset(objName) as GameObject;
    }

    /// <summary>
    /// LoadinGroupAsset
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="obj"></param>
    void OnLoadinGroupAsset(string loadPath, out GameObject[] obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAllAssets<GameObject>() as GameObject[];        
    }
}
