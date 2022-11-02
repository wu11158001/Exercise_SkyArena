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

    [Header("AttackBehavior")]
    [Tooltip("AttackList")] public List<AttackBehavior> attack_List = new List<AttackBehavior>();

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
        OnCreateInitialObject_List("BossObject", AssetManagement.Instance.boss_List);
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
    /// CreateInitialObject_List
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="list"></param>
    void OnCreateInitialObject_List(string objName, List<GameObject[]> list)
    {
        int number = 0;
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Length; j++)
            {
                number = objectPool.OnCreateAndRecordObject(list[i][j], FindChild.OnFindChild<Transform>(transform, "ObjectPool"));
                objectPool_Dictionary.Add($"{objName}{i}-{j}", number);
            }
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
        OnDamageOverTime();
    }

    /// <summary>
    /// DamageOverTime
    /// </summary>
    void OnDamageOverTime()
    {
        for (int i = 0; i < attack_List.Count; i++)
        {
            attack_List[i].OnDamageOverTime();
        }
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

        for (int i = 0; i < objs.Length; i++)
        {
            objectPool.OnCleanObject(OnSerchObjectPoolNumber(objectName + i.ToString()));
        }

        OnCleanEffect();
    }

    /// <summary>
    /// CleanBoss
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="list"></param>
    public void OnCleanBoss(string objectName, List<GameObject[]> list)
    {
        enemy_List.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Length; j++)
            {
                objectPool.OnCleanObject(OnSerchObjectPoolNumber($"{objectName}{i}-{j}"));
            }
        }

        OnCleanEffect();
    }

    /// <summary>
    /// CleanEffect
    /// </summary>
    void OnCleanEffect()
    {
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
        attack_List.Clear();

        GameObject player = objectPool.OnActiveObject(OnSerchObjectPoolNumber("PlayerObject"));
        player.layer = LayerMask.NameToLayer("Player");
        player.transform.position = Vector3.zero;
        if (!player.TryGetComponent<AIPlayer>(out aiPlayer)) aiPlayer = player.AddComponent<AIPlayer>();
        aiPlayer.OnInitialNumericalValue();   
        CameraControl.Instance.SetFollowTarget = player.transform;
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

        int bossType = 2;
        int bossNumber = 0;
        //int bossType = UnityEngine.Random.Range(0, AssetManagement.Instance.boss_List.Count);
        //int bossNumber = UnityEngine.Random.Range(0, AssetManagement.Instance.boss_List[bossType].Length);

        GameObject boss = objectPool.OnActiveObject(OnSerchObjectPoolNumber($"BossObject{bossType}-{bossNumber}"));
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
    /// CreateHitNumber
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

    #region Create Effect
    /// <summary>
    /// SearchEffect
    /// </summary>
    /// <param name="effectName"></param>
    GameObject  OnSearchEffect(string effectName)
    {
        GameObject effect = null;

        for (int i = 0; i < AssetManagement.Instance.effectObjects.Length; i++)
        {
            if (AssetManagement.Instance.effectObjects[i].name == effectName)
            {
                effect = objectPool.OnActiveObject(OnSerchObjectPoolNumber("EffectObject" + i.ToString()));
                if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effectLifeTime = effect.AddComponent<EffectLifeTime>();
                return effect;
            }
        }

        return default;
    }

    /// <summary>
    /// CreateEffect_Generally
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="effectName"></param>
    public void OnCreateEffect_Generally(Vector3 pos, string effectName)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return;
        
        effect.transform.position = pos;
    }

    /// <summary>
    /// CreateEffect_DamageOverTime
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="effectName"></param>
    public GameObject OnCreateEffect_DamageOverTime(Transform parent, string effectName)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return null;
        
        effect.transform.SetParent(parent);
        effect.transform.position = parent.position;
        effect.transform.rotation = parent.rotation;

        return effect;
    }

    /// <summary>
    /// CreateEffect_CollisionAttack
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="effectName"></param>
    /// <param name="attacker"></param>
    /// <param name="attackerRace"></param>
    /// <param name="attackPower"></param>
    public GameObject OnCreateEffect_CollisionAttack(Transform parent, string effectName, Transform attacker, AIPlayer.Race attackerRace, int attackPower)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return null;

        effect.transform.SetParent(parent);
        effect.transform.position = parent.position;
        effect.transform.rotation = parent.rotation;

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectCollisionAttack>(out EffectCollisionAttack effectCollisionAttack))
        {
            effectCollisionAttack = effect.AddComponent<EffectCollisionAttack>();            
        }
        effectCollisionAttack.attacker = attacker;
        effectCollisionAttack.attackerRace = attackerRace;
        effectCollisionAttack.attackPower = attackPower;

        return effect;
    }
    #endregion
}

