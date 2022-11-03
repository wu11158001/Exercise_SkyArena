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
    [Tooltip("hitTextObject")] public GameObject hitTextObject;
    [Tooltip("effectObjects")] public GameObject[] effectObjects;

    [Header("BossObject")]
    [Tooltip("bossObjects")] public List<GameObject[]> boss_List = new List<GameObject[]>();
    [Tooltip("bossObjects1")] public GameObject[] bossObjects1;
    [Tooltip("bossObjects2")] public GameObject[] bossObjects2;
    [Tooltip("bossObjects3")] public GameObject[] bossObjects3;
    [Tooltip("bossObjects4")] public GameObject[] bossObjects4;

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
        OnLoadinSingleAsset(loadPath: "prefab/hit_text", objName: "Hit_Text", obj: out hitTextObject);
        OnLoadinGroupAsset(loadPath: "prefab/enemysoldiers", obj: out enemySoldierObjects, null);                
        OnLoadinGroupAsset(loadPath: "prefab/effect", obj: out effectObjects, null);

        //boss
        OnLoadinGroupAsset(loadPath: "prefab/boss1", obj: out bossObjects1, boss_List);
        OnLoadinGroupAsset(loadPath: "prefab/boss2", obj: out bossObjects2, boss_List);
        OnLoadinGroupAsset(loadPath: "prefab/boss3", obj: out bossObjects3, boss_List);
        OnLoadinGroupAsset(loadPath: "prefab/boss4", obj: out bossObjects4, boss_List);
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
    void OnLoadinGroupAsset(string loadPath, out GameObject[] obj, List<GameObject[]> list)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAllAssets<GameObject>() as GameObject[];

        if (list != null) list.Add(obj);
    }
}
