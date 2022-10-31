using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 遊戲管理中心
/// </summary>
public class GameManagement : MonoBehaviour
{
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;

    [Header("Component")]
    ObjectPool objectPool = new ObjectPool();

    [Header("產生敵人")]
    [SerializeField] [Tooltip("地形物件")] Transform terrainObject;
    [SerializeField] [Tooltip("產生最大範圍")] float createMaxRadius = 4.3f;    
    [SerializeField] [Tooltip("產生時間(計時器)")] float createTimeCountDown = 0;

    [Header("物件")]
    [SerializeField] [Tooltip("玩家AI腳本")] AIPlayer aiPlayer;
    [SerializeField] [Tooltip("場上的敵人List")] List<Transform> enemy_List = new List<Transform>();
    [Tooltip("物件池物件")] Dictionary<string, int> objectPool_Dictionary = new Dictionary<string, int>();

    [Header("判斷")]    
    [Tooltip("玩家死亡")] public bool isPlayerDeath;
    [Tooltip("挑戰Boss")] public bool isChallengeBoss;

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
        OnTerrainPosition();//地形位置
        OnCreateInitialObject();//產生初始物件
        OnRespawnPlayer();//重生玩家
    }

    /// <summary>
    /// 地形位置
    /// </summary>
    void OnTerrainPosition()
    {
        terrainObject.position = new Vector3(0, -0.26f, 0);
    }

    /// <summary>
    /// 產生初始物件
    /// </summary>
    void OnCreateInitialObject()
    {
        objectPool = ObjectPool.Instance;
                
        OnCreateInitialObject_Single("PlayerObject", AssetManagement.Instance.playerObject);//主角物件
        OnCreateInitialObject_Group("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//敵人士兵物件
        OnCreateInitialObject_Group("BossObject", AssetManagement.Instance.bossObjects);//Boss物件
        OnCreateInitialObject_Single("HitTextObject", AssetManagement.Instance.hitTextObject);//擊中數字物件
        OnCreateInitialObject_Group("EffectObject", AssetManagement.Instance.effectObjects);//特效物件
    }    

    /// <summary>
    /// 產生初始物件_單一物件
    /// </summary>
    /// <param name="objName">物件名稱</param>
    /// <param name="obj">物件</param>
    void OnCreateInitialObject_Single(string objName, GameObject obj)
    {
        int number = 0;
        number = objectPool.OnCreateAndRecordObject(obj, FindChild.OnFindChild<Transform>(transform, "ObjectPool"));
        objectPool_Dictionary.Add(objName, number);
    }

    /// <summary>
    /// 產生初始物件_陣列物件
    /// </summary>
    /// <param name="objName">物件名稱</param>
    /// <param name="objs">物件</param>
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
    /// 搜尋物件池編號
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
        OnCreateEnenySoldier();//產生敵人士兵
    }

    /// <summary>
    /// 獲取玩家物件
    /// </summary>
    public AIPlayer GetPlayerObject => aiPlayer;

    /// <summary>
    /// 獲取敵人List
    /// </summary>
    public List<Transform> GetEnemyList => enemy_List;

    /// <summary>
    /// 清除敵人
    /// </summary>
    /// <param name="objectName">物件名稱</param>
    /// <param name="objs">物件</param>
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
    /// 重生玩家
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
    /// 產生敵人士兵
    /// </summary>
    void OnCreateEnenySoldier()
    {
        if (aiPlayer == null || !aiPlayer.gameObject.activeSelf) return;
        if (isChallengeBoss) return;

        createTimeCountDown -= Time.deltaTime;
        if (createTimeCountDown <= 0)
        {
            createTimeCountDown = UnityEngine.Random.Range(NumericalValueManagement.NumericalValueManagement.createEnemyTime[0], NumericalValueManagement.NumericalValueManagement.createEnemyTime[1]);

            //產生敵人士兵
            int enemyNumber = UnityEngine.Random.Range(0, AssetManagement.Instance.enemySoldierObjects.Length);
            GameObject enemy = objectPool.OnActiveObject(OnSerchObjectPoolNumber("EnemySoldierObject" + enemyNumber));
            enemy.layer = LayerMask.NameToLayer("Enemy");
            enemy.transform.position = OnEnemyInitialPosition();
            if (!enemy.TryGetComponent<AIEnemySoldier>(out AIEnemySoldier aiEnemySoldier)) aiEnemySoldier = enemy.AddComponent<AIEnemySoldier>();
            aiEnemySoldier.OnInitialNumericalValue();//初始數值            

            //紀錄敵人            
            enemy_List.Add(enemy.transform);
        }
    }

    /// <summary>
    /// 產生Boss
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
        aiBoss.OnInitialNumericalValue();//初始數值

        //紀錄敵人            
        enemy_List.Add(aiBoss.transform);
    }

    /// <summary>
    /// 敵人初始位置
    /// </summary>
    Vector3 OnEnemyInitialPosition()
    {
        float x = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        float y = 0;
        float z = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 產生擊中數字
    /// </summary>
    /// <param name="attacker">攻擊者物件</param>
    /// <param name="pos">初始位置</param>
    /// <param name="race">攻擊者種族</param>
    /// <param name="text">顯示文字</param>
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
    /// 產生特效
    /// </summary>
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

