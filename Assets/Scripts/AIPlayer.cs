using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���aAI
/// </summary>
public class AIPlayer : MonoBehaviour
{
    public enum Race { Player, Enemy}
    [Header("�ر�")]
    [SerializeField] [Tooltip("�ر�")] public Race race;

    protected enum State { Idle, Move, Attack}
    [Header("���A")]
    [SerializeField] [Tooltip("���A")] protected State state;   

    [Header("�ƭ�")]
    [SerializeField] [Tooltip("�ͩR��")] protected int Hp;
    [SerializeField] [Tooltip("�����O")] protected int attack;

    [Header("Component")]
    [SerializeField] [Tooltip("Animator")] Animator animator;
    [SerializeField] [Tooltip("AnimatorStateInfo")] AnimatorStateInfo info;
    [SerializeField] [Tooltip("�I����")]  CapsuleCollider thisCollider;

    [Header("����")]
    [SerializeField] [Tooltip("������H")] protected Transform targetObject;
    [SerializeField] [Tooltip("���ʳt��")] public float moveSpeed;
    [SerializeField] [Tooltip("����t��")] float rotateSpeed;
    [SerializeField] [Tooltip("�O�_�P�ĤH�I��")] bool isCollisionFromEnemy;

    [Header("����")]
    [SerializeField] [Tooltip("�i�ϥΧ����ۦ��ƶq")] protected int attackCount;
    [SerializeField] [Tooltip("�����b�|")] protected float attackRadius;
    [SerializeField] [Tooltip("�����W�v")] protected float attackFrequency;
    [SerializeField] [Tooltip("�����ɶ�(�p�ɾ�)")] float attackTimeCountDown;

    protected void Awake()
    {        
        //Component
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        //����        
        rotateSpeed = NumericalValueManagement.NumericalValue_Commom.commomValue_RotateSpeed;//����t��

        OnInitialNumericalValue();//��l�ƭ�
        playerInitial();//���a��l���
    }

    /// <summary>
    /// ��l�ƭ�
    /// </summary>
    public virtual void OnInitialNumericalValue()
    {        
        state = State.Idle;//���A

        race = Race.Player;//�ر�       
 
        moveSpeed = NumericalValueManagement.NumericalValue_Player.moveSpeed;//���ʳt��
        attackCount = NumericalValueManagement.NumericalValue_Player.attackCount;//�i�ϥΧ����ۦ��ƶq
        attackRadius = NumericalValueManagement.NumericalValue_Player.attackRadius;//�����b�|
        attackFrequency = NumericalValueManagement.NumericalValue_Player.attackFrequency;//�����W�v
        attackTimeCountDown = attackFrequency;        
    }

    /// <summary>
    /// ��s�ƭ�
    /// </summary>
    public virtual void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_Player.initial_Hp + 
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1));//�ͩR��
        attack = NumericalValueManagement.NumericalValue_Player.initial_Attack +
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeAttack * (GameDataManagement.Instance.playerLevel - 1));//�����O

        GameUI.Instance.OnSetPlayerLifeBar(Hp);////�]�w���a��� 
    }

    /// <summary>
    /// ���a��l���
    /// </summary>
    void playerInitial()
    {
        if (race == Race.Player)
        {
            GameUI.Instance.OnSetPlayerLevel();//�]�w���a����
            GameUI.Instance.OnSetPlayerExperience();//�]�w���a�g���
                             
            OnUpdateValue();//��s�ƭ�
        }
    }

    private void Update()
    {
        OnAnimatorStateInfo();//AnimatorStateInfo
        OnCheckTarget();//�ˬd�ؼЬO�_�s�b
        OnAnimationOver();//�ʵe����

        if (targetObject != null && targetObject.gameObject.activeSelf && Hp > 0)
        {
            OnMovement();//����
            OnAttackActive();//����
        }        
    }

    /// <summary>
    /// �E������
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
                //�P�_����
                Vector3 dir = (targetObject.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dir) < 0 || Vector3.Angle(transform.forward, dir) > 1) return;
                
                attackTimeCountDown = attackFrequency;

                int attackNumber = UnityEngine.Random.Range(1, attackCount + 1);
                animator.SetFloat("AttackNumber", attackNumber);                
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    protected void OnMovement()
    {
        //����
        if (!info.IsTag("Attack") && !isCollisionFromEnemy) transform.forward = Vector3.RotateTowards(transform.forward, targetObject.position - transform.position, rotateSpeed, 0);        

        //����
        if (!OnJudgeAttackRange())
        {
            if (state != State.Move)
            {
                state = State.Move;
                animator.SetBool("Move", true);
            }            
        }

        //�Z���ؼФӪ�
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
    /// �P�_�����d��
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

        OnJudgeEnemyCollider();//�P�_�P�ĤH�I��
        if(isCollisionFromEnemy && !isInRange) isCollisionFromEnemy = isInRange;
        
        return isInRange;
    }

    /// <summary>
    /// �P�_�P�ĤH�I��
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
    /// �ˬd�ؼЬO�_�s�b
    /// </summary>
    protected virtual void OnCheckTarget()
    {
        if (targetObject == null || !targetObject.gameObject.activeSelf)
        {
            OnSearchTarget();//�j�M�����ؼ�
        }
    }

    /// <summary>
    /// �j�M�����ؼ�
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
    /// �ʵe����
    /// </summary>
    protected void OnAnimationOver()
    {
        if (info.normalizedTime >= 1)
        {
            if (info.IsTag("Attack")) animator.SetFloat("AttackNumber", 0);//����
            if (info.IsTag("Death"))
            {
                gameObject.SetActive(false);//���`            
                OnBossDeath();//Boss���`
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
    /// ��������欰
    /// </summary>
    /// <param name="effectName">�S�ĦW��</param>
    void OnSingleAttackBehavior(string effectName)
    {
        new AttackBehavior().OnSingleAttack(attacker: transform,//�����̪���
                                            attackerRace: race,//�����̺ر�
                                            target: targetObject,//�����ؼ�
                                            attack: attack,//�����O
                                            effectName: effectName);//�S�ĦW��
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="attacker">�����̪���</param>
    /// <param name="attackerRace">�����̺ر�</param>
    /// <param name="attack">�����O</param>
    /// <param name="effectName">�S�ĦW��</param>
    public void OnGetHit(Transform attacker, Race attackerRace, int attack, string effectName)
    {
        if (attackerRace == race) return;//�����̺ر�
        if (Hp <= 0) return;        

        Hp -= attack;

        //���`
        if (Hp <= 0)
        {
            Hp = 0;
            animator.SetTrigger("Death");
            OnEnemyDeath();//�ĤH���`
            OnPlayerDeath();//���a���`
            if (attacker.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnSearchTarget();//�j�M�����ؼ�
        }

        //���������Ʀr
        GameManagement.Instance.OnCreateHitNumber(attacker: attacker,//�����̪���
                                                  pos: transform.position + thisCollider.center,//�Q�����̪���
                                                  race: race,//�Q�����̺ر�
                                                  text: attack.ToString());//��ܤ�r

        //���ͯS��
        if(!string.IsNullOrEmpty(effectName)) GameManagement.Instance.OnCreateEffect(transform.position + thisCollider.center, effectName);//���ͯS��

        OnPlayerGetHit();//���a����
        OnBossGetHit();//Boss����
    }   
    
    /// <summary>
    /// Boss����
    /// </summary>
    void OnBossGetHit()
    {
        if (race == Race.Enemy && GameManagement.Instance.isChallengeBoss)
        {
            GameUI.Instance.OnSetBossLifeBar(Hp);//�]�w���a���
        }
    }

    /// <summary>
    /// ���a����
    /// </summary>
    void OnPlayerGetHit()
    {
        //�Ӫ��󬰪��a
        if (race == Race.Player)
        {
            OnPlayerLifeBar(Hp);//���a���
        }
    }

    /// <summary>
    /// ���a���
    /// </summary>
    /// <param name="playerHp">���aHp</param>
    void OnPlayerLifeBar(int playerHp)
    {               
        GameUI.Instance.OnSetPlayerLifeBar(playerHp);//�]�w���a���
    }

    /// <summary>
    /// ���a���`
    /// </summary>
    void OnPlayerDeath()
    {
        if (race == Race.Player)
        {
            GameManagement.Instance.isPlayerDeath = true;
            GameUI.Instance.OnUIActive(false);//UI�E��
        }
    }

    /// <summary>
    /// �ĤH���`
    /// </summary>
    void OnEnemyDeath()
    {
        //�Ӫ��󬰪��a
        if (race == Race.Enemy)
        {
            //���a�g���
            GameDataManagement.Instance.playerExperience += NumericalValueManagement.NumericalValueManagement.enemyExperience;
            
            if(GameDataManagement.Instance.playerExperience >= ((GameDataManagement.Instance.playerLevel - 1) * NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience) + NumericalValueManagement.NumericalValueManagement.upgradeExperience)
            {
                OnPlayerUpgrade();//���a�ɯ�
            }
            GameUI.Instance.OnSetPlayerExperience();//�]�w���a�g���
            GameManagement.Instance.GetEnemyList.Remove(transform);//�����ĤHList 
        }
    }

    /// <summary>
    /// Boss���`
    /// </summary>
    void OnBossDeath()
    {
        //Boss
        if (GameManagement.Instance.isChallengeBoss && race == Race.Enemy)
        {
            GameManagement.Instance.isChallengeBoss = false;
            GameManagement.Instance.OnCleanEnemySoldier("BossObject", AssetManagement.Instance.bossObjects);//�M���ĤH
            GameManagement.Instance.GetPlayerObject.OnUpdateValue();//��s�ƭ�
            GameUI.Instance.OnSetGameLevel();//�]�w���d����
            GameUI.Instance.OnUIActive(true);//UI�E��                                             
        }
    }

    /// <summary>
    /// ���a�ɯ�
    /// </summary>
    void OnPlayerUpgrade()
    {        
        //�g�紣��        
        GameUI.Instance.OnSetPlayerLevel();//�]�w���a����        
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();//��s�ƭ�
    }    
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + thisCollider.center, attackRadius);
    }
}
