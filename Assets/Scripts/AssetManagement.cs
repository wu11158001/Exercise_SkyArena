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
    [Tooltip("BackgroundMusic")] public AudioClip[] backgroundMusic;

    private void Awake()
    {
        if(assetManagement != null)
        {
            Destroy(this);
            return;
        }
        assetManagement = this;
        DontDestroyOnLoad(gameObject);

        OnLoadAsset();
    }

    /// <summary>
    /// LoadAsset
    /// </summary>
    void OnLoadAsset()
    {        
        OnLoadingAsset_Single<GameObject>(loadPath: "prefab/player", objName: "Player", item: out playerObject);
        OnLoadingAsset_Single<GameObject>(loadPath: "prefab/texteffect", objName: "TextEffect", item: out textEffectObject);
        enemySoldierObjects = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/enemysoldiers");
        effectObjects = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/effect");

        //Boss
        bossObjects1 = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/boss1");
        bossObjects2 = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/boss2");
        bossObjects3 = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/boss3");
        bossObjects4 = OnLoadingAssets_Group<GameObject>(loadPath: "prefab/boss4");
        boss_List = new List<GameObject[]>() { bossObjects1, bossObjects2, bossObjects3, bossObjects4 };

        //Sprite               
        afkButtonSprite = OnLoadAssets_Sprite(loadPath: "ui/afkbutton");
        skillIconSprite = OnLoadAssets_Sprite(loadPath: "ui/skillicon");

        //Sound
        soundEffects = OnLoadingAssets_Group<AudioClip>(loadPath: "sound/soundeffect");
        backgroundMusic = OnLoadingAssets_Group<AudioClip>(loadPath: "sound/music");
    }   

    /// <summary>
    /// LoadingSingleAsset
    /// </summary>
    /// <param name="loadPath"></param>
    /// <param name="objName"></param>
    /// <param name="item"></param>
    void OnLoadingAsset_Single<T>(string loadPath, string objName, out T item)where T : Object
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        item = ab.LoadAsset(objName) as T;
    }

    /// <summary>
    /// OnLoadingAssets_Group
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="loadPath"></param>
    /// <param name="items"></param>
    T[] OnLoadingAssets_Group<T>(string loadPath) where T : Object
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";

        AssetBundle ab = AssetBundle.LoadFromFile(path);
        object[] objs = ab.LoadAllAssets() as object[];
        T[] temporaryItems = new T[objs.Length];
        for (int i = 0; i < temporaryItems.Length; i++)
        {
            temporaryItems[i] = objs[i] as T;
        }

        return temporaryItems;
    }

    /// <summary>
    /// LoadAssets_Sprite
    /// </summary>
    /// <param name="loadPath"></param>
    /// <returns></returns>
    Sprite[] OnLoadAssets_Sprite(string loadPath)
    {
        string path = $"{Application.streamingAssetsPath}/MyAssetBundle/{loadPath}";
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        object[] objs = ab.LoadAllAssets() as object[];       

        Texture2D[] texture2Ds = new Texture2D[objs.Length];
        Sprite[] sprites = new Sprite[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            texture2Ds[i] = objs[i] as Texture2D;
            sprites[i] = Sprite.Create(texture2Ds[i], new Rect(0, 0, texture2Ds[i].width, texture2Ds[i].height), Vector2.zero);
        }

        return sprites;
    }

    /// <summary>
    /// SearchAssets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="searchName"></param>
    /// <param name="searchArray"></param>
    /// <returns></returns>
    public T OnSearchAssets<T>(string searchName, T[] searchArray)where T:Object
    {
        if (searchArray == null) return default;

        for (int i = 0; i < searchArray.Length; i++)
        {
            if (searchArray[i].name == searchName)
            {
                return searchArray[i];
            }
        }

        return default;
    }
}
