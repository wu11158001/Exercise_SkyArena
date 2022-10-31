using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagement : MonoBehaviour
{
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;

    [Header("Component")]
    ObjectPool objectPool = new ObjectPool();

    [Header("Terrain")]
    [SerializeField] [Tooltip("TerrainObject")] Transform terrainObject;
    [SerializeField] [Tooltip("CreateMaxRadius")] float createMaxRadius = 4.3f;    
    [SerializeField] [Tooltip("CreateTimeCountDown")] float createTimeCountDown = 0;

    [Header("GameObjectList")]
    [SerializeField] [Tooltip("aiPlayer(Component)")] AIPlayer aiPlayer;
    [SerializeField] [Tooltip("enemy_List")] List<Transform> enemy_List = new List<Transform>();
    [Tooltip("objectPool_Dictionary")] Dictionary<string, int> objectPool_Dictionary = new Dictionary<string, int>();

    [Header("Judge")]    
    [Tooltip("isPlayerDeath")] public bool isPlayerDeath;
    [Tooltip("isChallengeBoss")] public bool isChallengeBoss;

    private void Awake()
    {
        if(AssetManagement.Instance == null)
        {
            SceneManager.LoadScene("StartScene");
            return;
        }
                
        if (gameManagement != null)
        {
            Destroy(this);
            return;
        }
        gameManagement = this;
    }

    private void Start()
    {
        OnTerrainPosition();
        OnCreateInitialObject();
        OnRespawnPlayer();
    }

    /// <summary>
    /// TerrainPosition
    /// </summary>
    void OnTerrainPosition()
    {
        terrainObject.position = new Vector3(0, -0.26f, 0);
    }

    /// <summary>
    /// CreateInitialObject
    /// </summary>
    void OnCreateInitialObject()
    {
        objectPool = ObjectPool.Instance;
                
        OnCreateInitialObject_Single("PlayerObject", AssetManagement.Instance.playerObject);
        OnCreateInitialObject_Group("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);
        OnCreateInitialObject_Group("BossObject", AssetManagement.Instance.bossObjects);
        OnCreateInitialObject_Single("HitTextObject", AssetManagement.Instance.hitTextObject);
        OnCreateInitialObject_Group("EffectObject", AssetManagement.Instance.effectObjects);
    }

    /// <summary>
    /// CreateInitialObject_Single
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="obj"></param>
    void OnCreateInitialObject_Single(string objName, GameObject obj)
    {
        int number = 0;
        number = objectPool.OnCreateAndRecordObject(obj, FindChild.OnFindChild<Transform>(transform, "ObjectPool"));
        objectPool_Dictionary.Add(objName, number);
    }

    /// <summary>
    /// CreateInitialObject_Group
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="objs"></param>
    void OnCreateInitialObject_Group(string objName, GameObject[] objs)
    {
        int number = 0;
        for (int i = 0; i < objs.Length; i++)
        {
            number = objectPool.OnCreateAndRecordObject(objs[i], FindChild.OnFindChild<Transform>(transform, "ObjectPool"));
            objectPool_Dictionary.Add(objName + i.ToString(), number);
        }
    }

    /// <summary>
    /// SerchObjectPoolNumber
    /// </summary>
    /// <param name="objectName"></param>
    int OnSerchObjectPoolNumber(string objectName)
    {
        int number = -1;

        foreach (var item in objectPool_Dictionary)
        {
            if(item.Key == objectName)
            {
                number = item.Value;
            }
        }

        return number;
    }

    private void Update()
    {
        OnCreateEnenySoldier();
    }

    /// <summary>
    /// GetPlayerObject
    /// </summary>
    public AIPlayer GetPlayerObject => aiPlayer;

    /// <summary>
    /// GetEnemyList
    /// </summary>
    public List<Transform> GetEnemyList => enemy_List;

    /// <summary>
    /// CleanEnemySoldier
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="objs"></param>
    public void OnCleanEnemySoldier(string objectName , GameObject[] objs)
    {
        enemy_List.Clear();

        //移除物件
        for (int i = 0; i < objs.Length; i++)
        {
            objectPool.OnCleanObject(OnSerchObjectPoolNumber(objectName + i.ToString()));
        }

        //移除特效
        for (int i = 0; i < AssetManagement.Instance.effectObjects.Length; i++)
        {
            objectPool.OnCleanObject(OnSerchObjectPoolNumber("EffectObject" + i.ToString()));
        }
    }

    /// <summary>
    /// RespawnPlayer
    /// </summary>
    public void OnRespawnPlayer()
    {
        GameObject player = objectPool.OnActiveObject(OnSerchObjectPoolNumber("PlayerObject"));
        player.layer = LayerMask.NameToLayer("Player");
        player.transform.position = Vector3.zero;
        if (!player.TryGetComponent<AIPlayer>(out aiPlayer)) aiPlayer = player.AddComponent<AIPlayer>();
        aiPlayer.OnInitialNumericalValue();//初始數值        
        CameraControl.Instance.SetFollowTarget = player.transform;//設定攝影機跟隨物件
    }

    /// <summary>
    /// CreateEnenySoldier
    /// </summary>
    void OnCreateEnenySoldier()
    {
        if (aiPlayer == null || !aiPlayer.gameObject.activeSelf) return;
        if (isChallengeBoss) return;

        createTimeCountDown -= Time.deltaTime;
        if (createTimeCountDown <= 0)
        {
            createTimeCountDown = UnityEngine.Random.Range(NumericalValueManagement.NumericalValue_Game.createEnemyTime[0], NumericalValueManagement.NumericalValue_Game.createEnemyTime[1]);
                     
            int enemyNumber = UnityEngine.Random.Range(0, AssetManagement.Instance.enemySoldierObjects.Length);
            GameObject enemy = objectPool.OnActiveObject(OnSerchObjectPoolNumber("EnemySoldierObject" + enemyNumber));
            enemy.layer = LayerMask.NameToLayer("Enemy");
            enemy.transform.position = OnEnemyInitialPosition();
            if (!enemy.TryGetComponent<AIEnemySoldier>(out AIEnemySoldier aiEnemySoldier)) aiEnemySoldier = enemy.AddComponent<AIEnemySoldier>();
            aiEnemySoldier.OnInitialNumericalValue();     
       
            enemy_List.Add(enemy.transform);
        }
    }

    /// <summary>
    /// CreateBoss
    /// </summary>
    public void OnCreateBoss()
    {
        if (aiPlayer == null || !aiPlayer.gameObject.activeSelf) return;

        isChallengeBoss = true;

        int bossNumber = UnityEngine.Random.Range(0, AssetManagement.Instance.bossObjects.Length);
        GameObject boss = objectPool.OnActiveObject(OnSerchObjectPoolNumber("BossObject" + bossNumber));
        boss.layer = LayerMask.NameToLayer("Enemy");
        boss.transform.position = OnEnemyInitialPosition();
        if (!boss.TryGetComponent<AIBoss>(out AIBoss aiBoss)) aiBoss = boss.AddComponent<AIBoss>();
        aiBoss.OnInitialNumericalValue();
       
        enemy_List.Add(aiBoss.transform);
    }

    /// <summary>
    /// EnemyInitialPosition
    /// </summary>
    Vector3 OnEnemyInitialPosition()
    {
        float x = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        float y = 0;
        float z = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// OnCreateHitNumber
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="pos"></param>
    /// <param name="race"></param>
    /// <param name="text"></param>
    public void OnCreateHitNumber(Transform attacker, Vector3 pos, AIPlayer.Race race, string text)
    {
        GameObject numberObject = objectPool.OnActiveObject(OnSerchObjectPoolNumber("HitTextObject"));
        if (!numberObject.TryGetComponent<HitText>(out HitText hitText)) hitText = numberObject.AddComponent<HitText>();

        //設定文字
        hitText.OnSetText(attackerForward: attacker.forward,//攻擊者物件前方
                          pos: pos,//初始位置
                          race: race,//攻擊者種族
                          text: text);//顯示文字
    }

    /// <summary>
    /// OnCreateEffect
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="effectName"></param>
    public void OnCreateEffect(Vector3 pos, string effectName)
    {
        for (int i = 0; i < AssetManagement.Instance.effectObjects.Length; i++)
        {
            if(AssetManagement.Instance.effectObjects[i].name == effectName)
            {
                GameObject effect = objectPool.OnActiveObject(OnSerchObjectPoolNumber("EffectObject" + i.ToString()));
                if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effect.AddComponent<EffectLifeTime>();                
                effect.transform.position = pos;

                return;
            }
        }        
    }
}

