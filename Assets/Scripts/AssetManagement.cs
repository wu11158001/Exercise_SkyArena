using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戈方恨瞶いみ
/// </summary>
public class AssetManagement : MonoBehaviour
{
    static AssetManagement assetManagement;
    public static AssetManagement Instance => assetManagement;

    [Header("ン")]
    [Tooltip("àン")] public GameObject playerObject;
    [Tooltip("寄ン")] public GameObject[] enemySoldierObjects;
    [Tooltip("Bossン")] public GameObject[] bossObjects;
    [Tooltip("阑い计ン")] public GameObject hitTextObject;
    [Tooltip("疭ン")] public GameObject[] effectObjects;

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
        OnLoadinSingleAsset(loadPath:"prefab/player", objName:"Player", obj: out playerObject);//更àン
        OnLoadinGroupAsset(loadPath: "prefab/enemysoldiers", obj: out enemySoldierObjects);//更寄ン
        OnLoadinGroupAsset(loadPath: "prefab/boss", obj: out bossObjects);//更Bossン
        OnLoadinSingleAsset(loadPath: "prefab/hit_text", objName: "Hit_Text", obj: out hitTextObject);//更阑い计ン
        OnLoadinGroupAsset(loadPath: "prefab/effect", obj: out effectObjects);//更疭ン
    }    

    /// <summary>
    /// 更虫縒戈方
    /// </summary>
    /// <param name="loadPath">更隔畖</param>
    /// <param name="objName">更嘿</param>
    /// <param name="obj">ン</param>
    void OnLoadinSingleAsset(string loadPath, string objName, out GameObject obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAsset(objName) as GameObject;
    }

    /// <summary>
    /// 更戈方
    /// </summary>
    /// <param name="loadPath">更隔畖</param>
    /// <param name="obj">ン</param>
    void OnLoadinGroupAsset(string loadPath, out GameObject[] obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAllAssets<GameObject>() as GameObject[];        
    }
}
