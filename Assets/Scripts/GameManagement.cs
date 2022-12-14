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

        terrainObject = GameObject.Find("Arena").transform;
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
        terrainObject.position = new Vector3(0, -0.5f, 1.2f);
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
        OnCreateInitialObject_Single("TextEffectObject", AssetManagement.Instance.textEffectObject);
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
        //attack_List.Clear();

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
        
        int bossType = GameDataManagement.Instance.selectBossType >= 0 ? 
            GameDataManagement.Instance.selectBossType : UnityEngine.Random.Range(0, AssetManagement.Instance.boss_List.Count);
        GameDataManagement.Instance.selectBossType = bossType;

        int bossNumber = GameDataManagement.Instance.selectBossNumber >= 0 ? 
            GameDataManagement.Instance.selectBossNumber : UnityEngine.Random.Range(0, AssetManagement.Instance.boss_List[bossType].Length);
        GameDataManagement.Instance.selectBossNumber = bossNumber;

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
    /// CreateTextEffect
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="text"></param>
    public void OnCreateTextEffect(Transform attacker, Vector3 position, Color color, string text, TextEffect.TextType type)
    {
        GameObject numberObject = objectPool.OnActiveObject(OnSerchObjectPoolNumber("TextEffectObject"));
        if (!numberObject.TryGetComponent<TextEffect>(out TextEffect hitText)) hitText = numberObject.AddComponent<TextEffect>();

        hitText.OnSetText(attackerForward: attacker.forward,
                          position: position,
                          color: color,
                          text: text,
                          type: type);
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
                return effect;
            }
        }

        return default;
    }

    /// <summary>
    /// CreateUpGradeEffect
    /// </summary>
    /// <param name="parent">parent</param>
    public void OnCreateUpGradeEffect(Transform parent)
    {
        GameObject effect = OnSearchEffect("UpGrade");
        if (effect == null) return;

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effectLifeTime = effect.AddComponent<EffectLifeTime>();
        effect.transform.SetParent(parent);
        effect.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// CreateEffect_Generally
    /// </summary>
    /// <param name="position"></param>
    /// <param name="effectName"></param>
    public void OnCreateEffect_Generally(Vector3 position, Vector3 forward,  string effectName)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return;

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effectLifeTime = effect.AddComponent<EffectLifeTime>();
        effect.transform.position = position;        
    }

    /// <summary>
    /// CreateEffect_DamageOverTime
    /// </summary>
    /// <param name="target"></param>
    /// <param name="effectName"></param>
    public GameObject OnCreateEffect_DamageOverTime(Transform target, string effectName)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return null;
                
        effect.transform.position = target.position;        

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effectLifeTime = effect.AddComponent<EffectLifeTime>();        

        return effect;
    }

    /// <summary>
    /// CreateEffect_CollisionAttack
    /// </summary>
    /// <param name="shootingPosition"></param>
    /// <param name="effectName"></param>
    /// <param name="attacker"></param>
    /// <param name="attackerRace"></param>
    /// <param name="attackPower"></param>
    public GameObject OnCreateEffect_CollisionAttack(Transform shootingPosition, string effectName, Transform attacker, AIPlayer.Race attackerRace, int attackPower)
    {
        if (shootingPosition == null)
        {
            Debug.LogError("shootingPosition is null");
            return null;
        }

        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return null;

        effect.transform.SetParent(shootingPosition);
        effect.transform.position = shootingPosition.position;
        effect.transform.rotation = shootingPosition.rotation;

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime)) effectLifeTime = effect.AddComponent<EffectLifeTime>();        
        if (!effect.TryGetComponent<EffectCollisionAttack>(out EffectCollisionAttack effectCollisionAttack))
        {
            effectCollisionAttack = effect.AddComponent<EffectCollisionAttack>();            
        }
        effectCollisionAttack.attacker = attacker;
        effectCollisionAttack.attackerRace = attackerRace;
        effectCollisionAttack.attackPower = attackPower;

        return effect;
    }

    /// <summary>
    /// CreateEffect_ObjectTrackAttack
    /// </summary>
    public void OnCreateEffect_ObjectTrackAttack(Transform shootingPosition, string effectName, Transform attacker, AIPlayer.Race attackerRace, int attackPower, Transform target)
    {
        if (shootingPosition == null)
        {
            Debug.LogError("shootingPosition is null");
            return;
        }

        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return;

        effect.transform.position = shootingPosition.position;
        effect.transform.rotation = Quaternion.Euler(Vector3.zero);
        effect.transform.forward = shootingPosition.forward;

        //Add EffectCollisionAttack
        if (!effect.TryGetComponent<EffectObjectTrackAttack>(out EffectObjectTrackAttack effectObjectTrackAttack))
        {
            effectObjectTrackAttack = effect.AddComponent<EffectObjectTrackAttack>();
        }
        effectObjectTrackAttack.target = target;
        effectObjectTrackAttack.rotateSpeed = 0;
        effectObjectTrackAttack.raiseRotateSpeed = 0;
        effectObjectTrackAttack.attacker = attacker;
        effectObjectTrackAttack.attackerRace = attackerRace;
        effectObjectTrackAttack.attackPower = attackPower;
    }

    /// <summary>
    /// CreateEffect_Loop
    /// </summary>
    /// <param name="position"></param>
    /// <param name="effectName"></param>
    /// <returns></returns>
    public GameObject OnCreateEffect_Loop(Vector3 position, string effectName)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return null;

        effect.transform.position = position;

        return effect;
    }

    /// <summary>
    /// CreateEffect_MoveForward
    /// </summary>
    /// <param name="position"></param>
    /// <param name="effectName"></param>
    /// <param name="damage"></param>
    /// <param name="attacker"></param>
    /// <param name="attackerRace"></param>
    public void OnCreateEffect_MoveForward(Vector3 position,string effectName, int damage, Transform attacker, AIPlayer.Race attackerRace)
    {
        GameObject effect = OnSearchEffect(effectName);
        if (effect == null) return;
        if (!effect.TryGetComponent<EffectMoveForward>(out EffectMoveForward effectMoveForward)) effectMoveForward = effect.AddComponent<EffectMoveForward>();
        effect.transform.position = position;
        effect.transform.rotation = attacker.rotation;
        effect.transform.forward = attacker.forward;
        effectMoveForward.damage = damage;
        effectMoveForward.attacker = attacker;
        effectMoveForward.attackerRace = attackerRace;
    }
    #endregion
}

