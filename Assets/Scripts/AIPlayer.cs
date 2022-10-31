using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 玩家AI
/// </summary>
public class AIPlayer : MonoBehaviour
{
    public enum Race { Player, Enemy}
    [Header("種族")]
    [SerializeField] [Tooltip("種族")] public Race race;

    protected enum State { Idle, Move, Attack}
    [Header("狀態")]
    [SerializeField] [Tooltip("狀態")] protected State state;   

    [Header("數值")]
    [SerializeField] [Tooltip("生命值")] protected int Hp;
    [SerializeField] [Tooltip("攻擊力")] protected int attack;

    [Header("Component")]
    [SerializeField] [Tooltip("Animator")] Animator animator;
    [SerializeField] [Tooltip("AnimatorStateInfo")] AnimatorStateInfo info;
    [SerializeField] [Tooltip("碰撞框")]  CapsuleCollider thisCollider;

    [Header("移動")]
    [SerializeField] [Tooltip("攻擊對象")] protected Transform targetObject;
    [SerializeField] [Tooltip("移動速度")] public float moveSpeed;
    [SerializeField] [Tooltip("選轉速度")] float rotateSpeed;
    [SerializeField] [Tooltip("是否與敵人碰撞")] bool isCollisionFromEnemy;

    [Header("攻擊")]
    [SerializeField] [Tooltip("可使用攻擊招式數量")] protected int attackCount;
    [SerializeField] [Tooltip("攻擊半徑")] protected float attackRadius;
    [SerializeField] [Tooltip("攻擊頻率")] protected float attackFrequency;
    [SerializeField] [Tooltip("攻擊時間(計時器)")] float attackTimeCountDown;

    protected void Awake()
    {        
        //Component
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        //移動        
        rotateSpeed = NumericalValueManagement.NumericalValue_Commom.commomValue_RotateSpeed;//選轉速度

        OnInitialNumericalValue();//初始數值
        playerInitial();//玩家初始資料
    }

    /// <summary>
    /// 初始數值
    /// </summary>
    public virtual void OnInitialNumericalValue()
    {        
        state = State.Idle;//狀態

        race = Race.Player;//種族       
 
        moveSpeed = NumericalValueManagement.NumericalValue_Player.moveSpeed;//移動速度
        attackCount = NumericalValueManagement.NumericalValue_Player.attackCount;//可使用攻擊招式數量
        attackRadius = NumericalValueManagement.NumericalValue_Player.attackRadius;//攻擊半徑
        attackFrequency = NumericalValueManagement.NumericalValue_Player.attackFrequency;//攻擊頻率
        attackTimeCountDown = attackFrequency;        
    }

    /// <summary>
    /// 更新數值
    /// </summary>
    public virtual void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_Player.initial_Hp + 
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1));//生命值
        attack = NumericalValueManagement.NumericalValue_Player.initial_Attack +
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeAttack * (GameDataManagement.Instance.playerLevel - 1));//攻擊力

        GameUI.Instance.OnSetPlayerLifeBar(Hp);////設定玩家血條 
    }

    /// <summary>
    /// 玩家初始資料
    /// </summary>
    void playerInitial()
    {
        if (race == Race.Player)
        {
            GameUI.Instance.OnSetPlayerLevel();//設定玩家等級
            GameUI.Instance.OnSetPlayerExperience();//設定玩家經驗值
                             
            OnUpdateValue();//更新數值
        }
    }

    private void Update()
    {
        OnAnimatorStateInfo();//AnimatorStateInfo
        OnCheckTarget();//檢查目標是否存在
        OnAnimationOver();//動畫結束

        if (targetObject != null && targetObject.gameObject.activeSelf && Hp > 0)
        {
            OnMovement();//移動
            OnAttackActive();//攻擊
        }        
    }

    /// <summary>
    /// 激活攻擊
    /// </summary>
    protected void OnAttackActive()
    {
        if (OnJudgeAttackRange() && !isCollisionFromEnemy)
        {
            if (targetObject == null || !targetObject.gameObject.activeSelf) return;                        
            
            if (state == State.Move) animator.SetBool("Move", false);
            state = State.Attack;

            if(!info.IsTag("Attack")) attackTimeCountDown -= Time.deltaTime;
            if(attackTimeCountDown <= 0)
            {
                //判斷角度
                Vector3 dir = (targetObject.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dir) < 0 || Vector3.Angle(transform.forward, dir) > 1) return;
                
                attackTimeCountDown = attackFrequency;

                int attackNumber = UnityEngine.Random.Range(1, attackCount + 1);
                animator.SetFloat("AttackNumber", attackNumber);                
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    protected void OnMovement()
    {
        //選轉
        if (!info.IsTag("Attack") && !isCollisionFromEnemy) transform.forward = Vector3.RotateTowards(transform.forward, targetObject.position - transform.position, rotateSpeed, 0);        

        //移動
        if (!OnJudgeAttackRange())
        {
            if (state != State.Move)
            {
                state = State.Move;
                animator.SetBool("Move", true);
            }            
        }

        //距離目標太近
        if(!info.IsTag("Attack") && isCollisionFromEnemy)
        {
            if (state != State.Move)
            {
                state = State.Move;
                animator.SetBool("Move", true);
            }

            transform.forward = Vector3.RotateTowards(transform.forward, transform.position - targetObject.position, rotateSpeed, 0);         
        }
    }    

    /// <summary>
    /// 判斷攻擊範圍
    /// </summary>
    protected bool OnJudgeAttackRange()
    {
        bool isInRange = false;

        string rival = race == Race.Player ? "Enemy" : "Player";
        LayerMask mask = LayerMask.GetMask(rival);
        Collider[] colliders = Physics.OverlapSphere(transform.position + thisCollider.center, attackRadius, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform == targetObject)
            {
                isInRange = true;
                break;
            }
        }      

        OnJudgeEnemyCollider();//判斷與敵人碰撞
        if(isCollisionFromEnemy && !isInRange) isCollisionFromEnemy = isInRange;
        
        return isInRange;
    }

    /// <summary>
    /// 判斷與敵人碰撞
    /// </summary>
    /// <returns></returns>
    void OnJudgeEnemyCollider()
    {
        if (race == Race.Player)
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            Collider[] colliders = Physics.OverlapSphere(transform.position + thisCollider.center, thisCollider.radius, 1<<LayerMask.NameToLayer("Enemy"));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform == targetObject)
                {
                    isCollisionFromEnemy = true;
                    return;
                }
            }
        }        
    }

    /// <summary>
    /// 檢查目標是否存在
    /// </summary>
    protected virtual void OnCheckTarget()
    {
        if (targetObject == null || !targetObject.gameObject.activeSelf)
        {
            OnSearchTarget();//搜尋攻擊目標
        }
    }

    /// <summary>
    /// 搜尋攻擊目標
    /// </summary>
    public virtual void OnSearchTarget()
    {
        if(GameManagement.Instance.GetEnemyList.Count <= 0)
        {
            targetObject = null;
            return;
        }

        List<Transform> enemys = GameManagement.Instance.GetEnemyList;
        if (enemys.Count > 0) targetObject = enemys[UnityEngine.Random.Range(0, enemys.Count)];
    }

    /// <summary>
    /// 動畫結束
    /// </summary>
    protected void OnAnimationOver()
    {
        if (info.normalizedTime >= 1)
        {
            if (info.IsTag("Attack")) animator.SetFloat("AttackNumber", 0);//攻擊
            if (info.IsTag("Death"))
            {
                gameObject.SetActive(false);//死亡            
                OnBossDeath();//Boss死亡
            }
        }
    }

    /// <summary>
    /// OnAnimatorStateInfo
    /// </summary>
    protected void OnAnimatorStateInfo()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
    }

    /// <summary>
    /// 單體攻擊行為
    /// </summary>
    /// <param name="effectName">特效名稱</param>
    void OnSingleAttackBehavior(string effectName)
    {
        new AttackBehavior().OnSingleAttack(attacker: transform,//攻擊者物件
                                            attackerRace: race,//攻擊者種族
                                            target: targetObject,//攻擊目標
                                            attack: attack,//攻擊力
                                            effectName: effectName);//特效名稱
    }

    /// <summary>
    /// 受擊
    /// </summary>
    /// <param name="attacker">攻擊者物件</param>
    /// <param name="attackerRace">攻擊者種族</param>
    /// <param name="attack">攻擊力</param>
    /// <param name="effectName">特效名稱</param>
    public void OnGetHit(Transform attacker, Race attackerRace, int attack, string effectName)
    {
        if (attackerRace == race) return;//攻擊者種族
        if (Hp <= 0) return;        

        Hp -= attack;

        //死亡
        if (Hp <= 0)
        {
            Hp = 0;
            animator.SetTrigger("Death");
            OnEnemyDeath();//敵人死亡
            OnPlayerDeath();//玩家死亡
            if (attacker.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnSearchTarget();//搜尋攻擊目標
        }

        //產生擊中數字
        GameManagement.Instance.OnCreateHitNumber(attacker: attacker,//攻擊者物件
                                                  pos: transform.position + thisCollider.center,//被攻擊者物件
                                                  race: race,//被攻擊者種族
                                                  text: attack.ToString());//顯示文字

        //產生特效
        if(!string.IsNullOrEmpty(effectName)) GameManagement.Instance.OnCreateEffect(transform.position + thisCollider.center, effectName);//產生特效

        OnPlayerGetHit();//玩家受擊
        OnBossGetHit();//Boss受擊
    }   
    
    /// <summary>
    /// Boss受擊
    /// </summary>
    void OnBossGetHit()
    {
        if (race == Race.Enemy && GameManagement.Instance.isChallengeBoss)
        {
            GameUI.Instance.OnSetBossLifeBar(Hp);//設定玩家血條
        }
    }

    /// <summary>
    /// 玩家受擊
    /// </summary>
    void OnPlayerGetHit()
    {
        //該物件為玩家
        if (race == Race.Player)
        {
            OnPlayerLifeBar(Hp);//玩家血條
        }
    }

    /// <summary>
    /// 玩家血條
    /// </summary>
    /// <param name="playerHp">玩家Hp</param>
    void OnPlayerLifeBar(int playerHp)
    {               
        GameUI.Instance.OnSetPlayerLifeBar(playerHp);//設定玩家血條
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    void OnPlayerDeath()
    {
        if (race == Race.Player)
        {
            GameManagement.Instance.isPlayerDeath = true;
            GameUI.Instance.OnUIActive(false);//UI激活
        }
    }

    /// <summary>
    /// 敵人死亡
    /// </summary>
    void OnEnemyDeath()
    {
        //該物件為玩家
        if (race == Race.Enemy)
        {
            //玩家經驗值
            GameDataManagement.Instance.playerExperience += NumericalValueManagement.NumericalValueManagement.enemyExperience;
            
            if(GameDataManagement.Instance.playerExperience >= ((GameDataManagement.Instance.playerLevel - 1) * NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience) + NumericalValueManagement.NumericalValueManagement.upgradeExperience)
            {
                OnPlayerUpgrade();//玩家升級
            }
            GameUI.Instance.OnSetPlayerExperience();//設定玩家經驗條
            GameManagement.Instance.GetEnemyList.Remove(transform);//移除敵人List 
        }
    }

    /// <summary>
    /// Boss死亡
    /// </summary>
    void OnBossDeath()
    {
        //Boss
        if (GameManagement.Instance.isChallengeBoss && race == Race.Enemy)
        {
            GameManagement.Instance.isChallengeBoss = false;
            GameManagement.Instance.OnCleanEnemySoldier("BossObject", AssetManagement.Instance.bossObjects);//清除敵人
            GameManagement.Instance.GetPlayerObject.OnUpdateValue();//更新數值
            GameUI.Instance.OnSetGameLevel();//設定關卡等級
            GameUI.Instance.OnUIActive(true);//UI激活                                             
        }
    }

    /// <summary>
    /// 玩家升級
    /// </summary>
    void OnPlayerUpgrade()
    {        
        //經驗提升        
        GameUI.Instance.OnSetPlayerLevel();//設定玩家等級        
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();//更新數值
    }    
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + thisCollider.center, attackRadius);
    }
}
