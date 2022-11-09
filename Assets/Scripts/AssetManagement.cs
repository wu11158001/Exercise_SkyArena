using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManagement : MonoBehaviour
{
    static AssetManagement assetManagement;
    public static AssetManagement Instance => assetManagement;

    [Header("AssetObjects")]
    [Tooltip("PlayerObject")] public GameObject playerObject;
    [Tooltip("EnemySoldierObjects")] public GameObject[] enemySoldierObjects;    
    [Tooltip("HitTextObject")] public GameObject textEffectObject;
    [Tooltip("EffectObjects")] public GameObject[] effectObjects;

    [Header("BossObject")]
    [Tooltip("BossObjects")] public List<GameObject[]> boss_List = new List<GameObject[]>();
    [Tooltip("BossObjects1")] public GameObject[] bossObjects1;
    [Tooltip("BossObjects2")] public GameObject[] bossObjects2;
    [Tooltip("BossObjects3")] public GameObject[] bossObjects3;
    [Tooltip("BossObjects4")] public GameObject[] bossObjects4;

    [Header("Sprite")]
    [Tooltip("AFKButtonSprite")] public Sprite[] afkButtonSprite;
    [Tooltip("SkillIconSprite")] public Sprite[] skillIconSprite;

    [Header("Sound")]
    [Tooltip("SoundEffect")] public AudioClip[] soundEffects;

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
        OnLoadingSingleAsset(loadPath:"prefab/player", objName:"Player", obj: out playerObject);
        OnLoadingSingleAsset(loadPath: "prefab/texteffect", objName: "TextEffect", obj: out textEffectObject);
        OnLoadingGroupAsset(loadPath: "prefab/enemysoldiers", objs: out enemySoldierObjects);                
        OnLoadingGroupAsset(loadPath: "prefab/effect", objs: out effectObjects);

        //Boss
        OnLoadingGroupAsset(loadPath: "prefab/boss1", objs: out bossObjects1);
        OnLoadingGroupAsset(loadPath: "prefab/boss2", objs: out bossObjects2);
        OnLoadingGroupAsset(loadPath: "prefab/boss3", objs: out bossObjects3);
        OnLoadingGroupAsset(loadPath: "prefab/boss4", objs: out bossObjects4);
        boss_List = new List<GameObject[]>() { bossObjects1, bossObjects2, bossObjects3, bossObjects4};

        //Sprite
        OnLoadingSprite(loadPath: "ui/afkbutton", out afkButtonSprite);
        OnLoadingSprite(loadPath: "ui/skillicon", out skillIconSprite);

        //Sound
        OnLoadingAudioClip(loadPath: "sound/soundeffect", out soundEffects);
    }

    /// <summary>
    /// LoadingSingleAsset
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objName"></param>
    /// <param name="obj"></param>
    void OnLoadingSingleAsset(string loadPath, string objName, out GameObject obj)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        obj = ab.LoadAsset(objName) as GameObject;
    }

    /// <summary>
    /// LoadingGroupAsset
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objs"></param>
    void OnLoadingGroupAsset(string loadPath, out GameObject[] objs)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        objs = ab.LoadAllAssets<GameObject>() as GameObject[];          
    }

    /// <summary>
    /// LoadingSprite
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objs"></param>
    void OnLoadingSprite(string loadPath, out Sprite[] objs)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        objs = ab.LoadAllAssets<Sprite>() as Sprite[];
    }

    /// <summary>
    /// LoadingAudioClip
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objs"></param>
    void OnLoadingAudioClip(string loadPath, out AudioClip[] objs)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        objs = ab.LoadAllAssets<AudioClip>() as AudioClip[];
    }

    /// <summary>
    /// SearchSound
    /// </summary>
    /// <param name="searchName"></param>
    /// <param name="searchArray"></param>
    /// <returns></returns>
    public AudioClip OnSearchSound(string searchName, AudioClip[] searchArray)
    {
        if (searchArray == null) return default;
                
        for (int i = 0; i < searchArray.Length; i++)
        {
            if(searchArray[i].name == searchName)
            {
                return searchArray[i];
            }
        }

        return default;
    }
}
