using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �C���޲z����
/// </summary>
public class GameManagement : MonoBehaviour
{
    static GameManagement gameManagement;
    public static GameManagement Instance => gameManagement;

    [Header("Component")]
    ObjectPool objectPool = new ObjectPool();

    [Header("���ͼĤH")]
    [SerializeField] [Tooltip("�a�Ϊ���")] Transform terrainObject;
    [SerializeField] [Tooltip("���ͳ̤j�d��")] float createMaxRadius = 4.3f;    
    [SerializeField] [Tooltip("���ͮɶ�(�p�ɾ�)")] float createTimeCountDown = 0;

    [Header("����")]
    [SerializeField] [Tooltip("���aAI�}��")] AIPlayer aiPlayer;
    [SerializeField] [Tooltip("���W���ĤHList")] List<Transform> enemy_List = new List<Transform>();
    [Tooltip("���������")] Dictionary<string, int> objectPool_Dictionary = new Dictionary<string, int>();

    [Header("�P�_")]    
    [Tooltip("���a���`")] public bool isPlayerDeath;
    [Tooltip("�D��Boss")] public bool isChallengeBoss;

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
        OnTerrainPosition();//�a�Φ�m
        OnCreateInitialObject();//���ͪ�l����
        OnRespawnPlayer();//���ͪ��a
    }

    /// <summary>
    /// �a�Φ�m
    /// </summary>
    void OnTerrainPosition()
    {
        terrainObject.position = new Vector3(0, -0.26f, 0);
    }

    /// <summary>
    /// ���ͪ�l����
    /// </summary>
    void OnCreateInitialObject()
    {
        objectPool = ObjectPool.Instance;
                
        OnCreateInitialObject_Single("PlayerObject", AssetManagement.Instance.playerObject);//�D������
        OnCreateInitialObject_Group("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//�ĤH�h�L����
        OnCreateInitialObject_Group("BossObject", AssetManagement.Instance.bossObjects);//Boss����
        OnCreateInitialObject_Single("HitTextObject", AssetManagement.Instance.hitTextObject);//�����Ʀr����
        OnCreateInitialObject_Group("EffectObject", AssetManagement.Instance.effectObjects);//�S�Ī���
    }    

    /// <summary>
    /// ���ͪ�l����_��@����
    /// </summary>
    /// <param name="objName">����W��</param>
    /// <param name="obj">����</param>
    void OnCreateInitialObject_Single(string objName, GameObject obj)
    {
        int number = 0;
        number = objectPool.OnCreateAndRecordObject(obj, FindChild.OnFindChild<Transform>(transform, "ObjectPool"));
        objectPool_Dictionary.Add(objName, number);
    }

    /// <summary>
    /// ���ͪ�l����_�}�C����
    /// </summary>
    /// <param name="objName">����W��</param>
    /// <param name="objs">����</param>
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
    /// �j�M������s��
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
        OnCreateEnenySoldier();//���ͼĤH�h�L
    }

    /// <summary>
    /// ������a����
    /// </summary>
    public AIPlayer GetPlayerObject => aiPlayer;

    /// <summary>
    /// ����ĤHList
    /// </summary>
    public List<Transform> GetEnemyList => enemy_List;

    /// <summary>
    /// �M���ĤH
    /// </summary>
    /// <param name="objectName">����W��</param>
    /// <param name="objs">����</param>
    public void OnCleanEnemySoldier(string objectName , GameObject[] objs)
    {
        enemy_List.Clear();

        //��������
        for (int i = 0; i < objs.Length; i++)
        {
            objectPool.OnCleanObject(OnSerchObjectPoolNumber(objectName + i.ToString()));
        }

        //�����S��
        for (int i = 0; i < AssetManagement.Instance.effectObjects.Length; i++)
        {
            objectPool.OnCleanObject(OnSerchObjectPoolNumber("EffectObject" + i.ToString()));
        }
    }

    /// <summary>
    /// ���ͪ��a
    /// </summary>
    public void OnRespawnPlayer()
    {
        GameObject player = objectPool.OnActiveObject(OnSerchObjectPoolNumber("PlayerObject"));
        player.layer = LayerMask.NameToLayer("Player");
        player.transform.position = Vector3.zero;
        if (!player.TryGetComponent<AIPlayer>(out aiPlayer)) aiPlayer = player.AddComponent<AIPlayer>();
        aiPlayer.OnInitialNumericalValue();//��l�ƭ�        
        CameraControl.Instance.SetFollowTarget = player.transform;//�]�w��v�����H����
    }   

    /// <summary>
    /// ���ͼĤH�h�L
    /// </summary>
    void OnCreateEnenySoldier()
    {
        if (aiPlayer == null || !aiPlayer.gameObject.activeSelf) return;
        if (isChallengeBoss) return;

        createTimeCountDown -= Time.deltaTime;
        if (createTimeCountDown <= 0)
        {
            createTimeCountDown = UnityEngine.Random.Range(NumericalValueManagement.NumericalValueManagement.createEnemyTime[0], NumericalValueManagement.NumericalValueManagement.createEnemyTime[1]);

            //���ͼĤH�h�L
            int enemyNumber = UnityEngine.Random.Range(0, AssetManagement.Instance.enemySoldierObjects.Length);
            GameObject enemy = objectPool.OnActiveObject(OnSerchObjectPoolNumber("EnemySoldierObject" + enemyNumber));
            enemy.layer = LayerMask.NameToLayer("Enemy");
            enemy.transform.position = OnEnemyInitialPosition();
            if (!enemy.TryGetComponent<AIEnemySoldier>(out AIEnemySoldier aiEnemySoldier)) aiEnemySoldier = enemy.AddComponent<AIEnemySoldier>();
            aiEnemySoldier.OnInitialNumericalValue();//��l�ƭ�            

            //�����ĤH            
            enemy_List.Add(enemy.transform);
        }
    }

    /// <summary>
    /// ����Boss
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
        aiBoss.OnInitialNumericalValue();//��l�ƭ�

        //�����ĤH            
        enemy_List.Add(aiBoss.transform);
    }

    /// <summary>
    /// �ĤH��l��m
    /// </summary>
    Vector3 OnEnemyInitialPosition()
    {
        float x = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        float y = 0;
        float z = UnityEngine.Random.Range(-createMaxRadius, createMaxRadius);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// ���������Ʀr
    /// </summary>
    /// <param name="attacker">�����̪���</param>
    /// <param name="pos">��l��m</param>
    /// <param name="race">�����̺ر�</param>
    /// <param name="text">��ܤ�r</param>
    public void OnCreateHitNumber(Transform attacker, Vector3 pos, AIPlayer.Race race, string text)
    {
        GameObject numberObject = objectPool.OnActiveObject(OnSerchObjectPoolNumber("HitTextObject"));
        if (!numberObject.TryGetComponent<HitText>(out HitText hitText)) hitText = numberObject.AddComponent<HitText>();

        //�]�w��r
        hitText.OnSetText(attackerForward: attacker.forward,//�����̪���e��
                          pos: pos,//��l��m
                          race: race,//�����̺ر�
                          text: text);//��ܤ�r
    }

    /// <summary>
    /// ���ͯS��
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

