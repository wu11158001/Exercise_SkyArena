using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIPlayer : MonoBehaviour
{
    public enum Race { Player, Enemy }
    [Header("Race")]
    [SerializeField] [Tooltip("Race")] public Race race;

    protected enum State { Idle, Move, Attack }
    [Header("State")]
    [SerializeField] [Tooltip("State")] protected State state;

    [Header("Value")]
    [SerializeField] [Tooltip("Hp")] public int Hp;
    [SerializeField] [Tooltip("MaxHp")] protected int MaxHp;
    [SerializeField] [Tooltip("AttackPower")] protected int attackPower;

    [Header("Component")]
    [Tooltip("Animator")] Animator animator;
    [Tooltip("AnimatorStateInfo")] AnimatorStateInfo info;
    [Tooltip("Collider")] CapsuleCollider thisCollider;
    [Tooltip("ShootingPosition")] Transform shootingPosition;
    [Tooltip("ThisAudioSource")] AudioSource thisAudioSource;

    [Header("Move")]
    [SerializeField] [Tooltip("TargetObject")] protected Transform targetObject;
    [SerializeField] [Tooltip("MoveSpeed")] public float moveSpeed;
    [SerializeField] [Tooltip("RotateSpeed")] float rotateSpeed;
    [SerializeField] [Tooltip("IsCollisionFromEnemy")] bool isCollisionFromEnemy;

    [Header("Attack")]
    [SerializeField] [Tooltip("AttackCount")] protected int attackCount;
    [SerializeField] [Tooltip("AttackRadius")] protected float attackRadius;
    [SerializeField] [Tooltip("AttackFrequency")] protected float attackFrequency;
    [SerializeField] [Tooltip("AttackTimeCountDown")] float attackTimeCountDown;

    [Header("BossExclusive")]
    [SerializeField] [Tooltip("AttackDistance")] protected float attackDistance;
    [SerializeField] [Tooltip("DamageOverTimeRadius")] protected float damageOverTimeRadius;    
    [SerializeField] [Tooltip("CollisionAttackObjects")] protected List<EffectLifeTime> collisionAttackObject_List = new List<EffectLifeTime>();

    protected void Awake()
    {
        //Component
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<CapsuleCollider>();
        shootingPosition = FindChild.OnFindChild<Transform>(transform, "ShootingPosition");
        thisAudioSource = GetComponent<AudioSource>();
        thisAudioSource.volume = 0.5f;
    }

    private void Start()
    {
        rotateSpeed = NumericalValueManagement.NumericalValue_Commom.commomValue_RotateSpeed;

        OnInitialNumericalValue();
        OnPlayerInitial();
    }

    /// <summary>
    /// InitialNumericalValue
    /// </summary>
    public virtual void OnInitialNumericalValue()
    {
        state = State.Idle;
        race = Race.Player;

        moveSpeed = NumericalValueManagement.NumericalValue_Player.moveSpeed;
        attackCount = NumericalValueManagement.NumericalValue_Player.attackCount;
        attackRadius = NumericalValueManagement.NumericalValue_Player.attackRadius;
        attackFrequency = NumericalValueManagement.NumericalValue_Player.attackFrequency;
        attackTimeCountDown = attackFrequency;
    }

    /// <summary>
    /// UpdateValue
    /// </summary>
    public virtual void OnUpdateValue()
    {
        MaxHp = NumericalValueManagement.NumericalValue_Player.initial_Hp +
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerGrade - 1));

        Hp = MaxHp;

        attackPower = NumericalValueManagement.NumericalValue_Player.initial_AttackPower +
            (NumericalValueManagement.NumericalValue_Player.raiseUpgradeAttack * (GameDataManagement.Instance.playerGrade - 1));

        GameUI.Instance.OnSetPlayerLifeBar(Hp);
    }

    /// <summary>
    /// PlayerInitial
    /// </summary>
    void OnPlayerInitial()
    {
        if (race == Race.Player)
        {
            GameUI.Instance.OnSetPlayerGrade();
            GameUI.Instance.OnSetPlayerExperience();
            GameUI.Instance.OnSetPlayerGold();
            GameUI.Instance.OnSetGameLevel();            

            OnUpdateValue();
        }
    }

    private void Update()
    {
        OnAnimatorStateInfo();
        OnCheckTarget();
        OnAnimationOver();

        if (targetObject != null && targetObject.gameObject.activeSelf && Hp > 0)
        {
            OnMovement();
            OnAttackActive();
        }
    }

    #region AttackMethod
    /// <summary>
    /// JudgeTargetObjectIsActive
    /// </summary>
    bool OnJudgeTargetObjectIsActive()
    {
        if (transform == null || !transform.gameObject.activeSelf || targetObject == null || !targetObject.gameObject.activeSelf) return false;

        return true;
    }

    /// <summary>
    /// SingleAttackBehavior
    /// </summary>
    /// <param name="effectName"></param>    
    void OnSingleAttackBehavior(string effectName)
    {
        if (!OnJudgeTargetObjectIsActive()) return;
        new AttackBehavior().OnSingleAttack(attacker: transform,
                                            attackerRace: race,
                                            target: targetObject,
                                            attackPower: attackPower,
                                            effectName: effectName,
                                            soundEffectName: "GetHit");
    }

    /// <summary>
    /// SetDamageOverTimeAttack_Boss
    /// </summary>
    /// <param name="effectName"></param>
    void OnSetDamageOverTimeAttack_Boss(string effectName)
    {
        if (!OnJudgeTargetObjectIsActive()) return;

        GameObject effect = GameManagement.Instance.OnCreateEffect_DamageOverTime(targetObject, effectName);
        if (!effect.TryGetComponent<EffectDamageOverTime>(out EffectDamageOverTime effectDamageOverTime)) effectDamageOverTime = effect.AddComponent<EffectDamageOverTime>();
        effectDamageOverTime.timeCountDown = 0;
        effectDamageOverTime.attacker = transform;
        effectDamageOverTime.attackerRace = race;
        effectDamageOverTime.attackPower = attackPower / 5;
    }

    /// <summary>
    /// SetDamageOverTimeAttack_Player
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="damage"></param>
    public void OnSetDamageOverTimeAttack_Skill(string effectName, int damage)
    {
        if (!OnJudgeTargetObjectIsActive()) return;

        GameObject effect = GameManagement.Instance.OnCreateEffect_DamageOverTime(targetObject, effectName);
        if(!effect.TryGetComponent<EffectDamageOverTime>(out EffectDamageOverTime effectDamageOverTime)) effectDamageOverTime = effect.AddComponent<EffectDamageOverTime>();
        effectDamageOverTime.timeCountDown = 0;
        effectDamageOverTime.attacker = transform;
        effectDamageOverTime.attackerRace = race;
        effectDamageOverTime.attackPower = damage;   
    }

    /// <summary>
    /// SetBounceAttack
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="damage"></param>
    public void OnSetBounceAttack(string effectName, int damage)
    {
        GameObject effect = GameManagement.Instance.OnCreateEffect_Loop(transform.position + (thisCollider.center * transform.localScale.x), effectName);
        if (!effect.TryGetComponent<EffectBounceAttack>(out EffectBounceAttack effectBounceAttack)) effectBounceAttack = effect.AddComponent<EffectBounceAttack>();
        effectBounceAttack.attacker = transform;
        effectBounceAttack.attackerRace = race;
        effectBounceAttack.attackPower = damage;
        effectBounceAttack.OnInitialValue();
    }

    /// <summary>
    /// CollisionAttack
    /// </summary>
    /// <param name="effectName"></param>
    void OnCollisionAttack(string effectName)
    {       
        if(shootingPosition == null)
        {
            Debug.LogError("shootingPosition is null");
            return;
        }

        if (transform == null || !transform.gameObject.activeSelf) return;

        if (GameManagement.Instance.OnCreateEffect_CollisionAttack(shootingPosition: shootingPosition,
                                                                                     effectName: effectName,
                                                                                     attacker: transform,
                                                                                     attackerRace: race,
                                                                                     attackPower: attackPower).TryGetComponent<EffectLifeTime>(out EffectLifeTime effectLifeTime))
        {
            collisionAttackObject_List.Add(effectLifeTime);
        }       
    }

    /// <summary>
    /// ObjectTrackAttack
    /// </summary>
    /// <param name="effectName"></param>
    void OnObjectTrackAttack(string effectName)
    {
        if (shootingPosition == null)
        {
            Debug.LogError("shootingPosition is null");
            return;
        }

        if (!OnJudgeTargetObjectIsActive()) return;

        GameManagement.Instance.OnCreateEffect_ObjectTrackAttack(shootingPosition: shootingPosition,
                                                                 effectName: effectName,
                                                                 attacker: transform,
                                                                 attackerRace: race,
                                                                 attackPower: attackPower,
                                                                 target: targetObject);
    }

    /// <summary>
    /// SingleRandomAttack
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="numberOfTime"></param>
    /// <param name="damage"></param>
    /// <param name="soundEffectName"></param>
    public void OnSingleRandomAttack(string effectName, int numberOfTime, int damage, string soundEffectName)
    {       
        StartCoroutine(ISingleRandomAttack(effectName, numberOfTime, damage, soundEffectName));
    }

    /// <summary>
    /// ISingleRandomAttack
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="numberOfTime"></param>
    /// <param name="damage"></param>
    /// <param name="soundEffectName"></param>
    /// <returns></returns>
    IEnumerator ISingleRandomAttack(string effectName, int numberOfTime, int damage, string soundEffectName)
    {
        for (int i = 0; i < numberOfTime; i++)
        {
            if (GameManagement.Instance.GetEnemyList.Count <= 0) break;

            new AttackBehavior().OnSingleAttack(attacker: transform,
                                                attackerRace: race,
                                                target: GameManagement.Instance.GetEnemyList[UnityEngine.Random.Range(0, GameManagement.Instance.GetEnemyList.Count)],
                                                attackPower: damage,
                                                effectName: effectName,
                                                soundEffectName: soundEffectName);

            yield return new WaitForSeconds(NumericalValueManagement.NumericalValue_Game.attackFrequency);
        }
    }

    /// <summary>
    /// MoveForwardCollisionAttack
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="damage"></param>    
    public void OnMoveForwardCollisionAttack(string effectName, int damage)
    {
        GameManagement.Instance.OnCreateEffect_MoveForward(position: transform.position + (thisCollider.center * transform.localScale.x),
                                                           effectName: effectName,
                                                           damage: damage,
                                                           attacker: transform,
                                                           attackerRace: race);
    }   

    /// <summary>
    /// RemoveAttackList
    /// </summary>
    void OnRemoveAttackList()
    {
        if (collisionAttackObject_List.Count > 0)
        {
            foreach (var item in collisionAttackObject_List)
            {
                item.lifeTimeCountDown = item.lifeTime;                
            }            
        }
    }
    #endregion

    /// <summary>
    /// AttackActive
    /// </summary>
    protected void OnAttackActive()
    {
        if (OnJudgeAttackRange() && !isCollisionFromEnemy)
        {
            if (targetObject == null || !targetObject.gameObject.activeSelf) return;

            if (state == State.Move) animator.SetBool("Move", false);
            state = State.Attack;

            if (!info.IsTag("Attack")) attackTimeCountDown -= Time.deltaTime;
            if (attackTimeCountDown <= 0)
            {
                Vector3 dir = (targetObject.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dir) < 0 || Vector3.Angle(transform.forward, dir) > 1) return;

                attackTimeCountDown = attackFrequency;

                int attackNumber = UnityEngine.Random.Range(1, attackCount + 1);
                animator.SetFloat("AttackNumber", attackNumber);
            }
        }
    }

    /// <summary>
    /// Movement
    /// </summary>
    protected void OnMovement()
    {
        if (!info.IsTag("Attack") && !isCollisionFromEnemy) transform.forward = Vector3.RotateTowards(transform.forward, targetObject.position - transform.position, rotateSpeed, 0);

        if (!OnJudgeAttackRange())
        {
            if (state != State.Move)
            {
                state = State.Move;
                animator.SetBool("Move", true);
            }
        }

        if (!info.IsTag("Attack") && isCollisionFromEnemy)
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
    /// JudgeAttackRange
    /// </summary>
    protected bool OnJudgeAttackRange()
    {
        bool isInRange = false;

        string rival = race == Race.Player ? "Enemy" : "Player";
        LayerMask mask = LayerMask.GetMask(rival);
        Collider[] colliders = Physics.OverlapSphere(transform.position + (thisCollider.center * transform.localScale.x), attackRadius, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform == targetObject)
            {
                isInRange = true;
                break;
            }
        }

        OnJudgeCollisionFromEnemy();
        if (isCollisionFromEnemy && !isInRange) isCollisionFromEnemy = isInRange;

        return isInRange;
    }

    /// <summary>
    /// JudgeCollisionFromEnemy
    /// </summary>
    /// <returns></returns>
    void OnJudgeCollisionFromEnemy()
    {
        if (race == Race.Player)
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            Collider[] colliders = Physics.OverlapSphere(transform.position + thisCollider.center, thisCollider.radius, 1 << LayerMask.NameToLayer("Enemy"));
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
    /// CheckTarget
    /// </summary>
    protected virtual void OnCheckTarget()
    {
        if (targetObject == null || !targetObject.gameObject.activeSelf)
        {
            OnSearchTarget();
        }
    }

    /// <summary>
    /// SearchTarget
    /// </summary>
    public virtual void OnSearchTarget()
    {
        if (GameManagement.Instance.GetEnemyList.Count <= 0)
        {
            targetObject = null;
            return;
        }

        List<Transform> enemys = GameManagement.Instance.GetEnemyList;
        if (enemys.Count > 0) targetObject = enemys[UnityEngine.Random.Range(0, enemys.Count)];
    }

    #region GrtHit
    /// <summary>
    /// GetHit
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="attackerRace"></param>
    /// <param name="attack"></param>
    /// <param name="effectName"></param>
    /// <param name="soundEffectName"></param>
    public void OnGetHit(Transform attacker, Race attackerRace, int attack, string effectName, string soundEffectName)
    {
        if (attackerRace == race) return;
        if (Hp <= 0) return;

        Hp -= attack;

        //Death
        if (Hp <= 0)
        {
            Hp = 0;
            animator.SetTrigger("Death");
            OnEnemyDeath();
            OnPlayerDeath();
            if (attacker.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnSearchTarget();
        }

        //SoundEffect
        if(thisAudioSource && race == Race.Enemy)
        {
            string soundEffect = soundEffectName == "" ? "GetHit" : soundEffectName;
            thisAudioSource.clip = AssetManagement.Instance.OnSearchAssets<AudioClip>(soundEffect, AssetManagement.Instance.soundEffects);
            if (!thisAudioSource.isPlaying) thisAudioSource.Play();            
        }

        //TextEffect
        GameManagement.Instance.OnCreateTextEffect(attacker: attacker,
                                                   position: transform.position + (thisCollider.center * transform.localScale.x),
                                                   color: race == Race.Player ? Color.red : Color.white,
                                                   text: attack.ToString(),
                                                   type: TextEffect.TextType.GoBack);

        //Effect
        if (!string.IsNullOrEmpty(effectName)) GameManagement.Instance.OnCreateEffect_Generally( position: transform.position + (thisCollider.center * transform.localScale.x),
                                                                                                 forward: attacker.forward, 
                                                                                                 effectName: effectName);

        OnPlayerGetHit();
        OnBossGetHit();
    }

    /// <summary>
    /// BossGetHit
    /// </summary>
    void OnBossGetHit()
    {
        if (race == Race.Enemy && GameManagement.Instance.isChallengeBoss)
        {
            GameUI.Instance.OnSetBossLifeBar(Hp);
        }
    }

    /// <summary>
    /// PlayerGetHit
    /// </summary>
    void OnPlayerGetHit()
    {
        if (race == Race.Player)
        {
            OnPlayerLifeBar(Hp);
        }
    }

    /// <summary>
    /// PlayerLifeBar
    /// </summary>
    /// <param name="playerHp"></param>
    void OnPlayerLifeBar(int playerHp)
    {
        GameUI.Instance.OnSetPlayerLifeBar(playerHp);
    }

    /// <summary>
    /// PlayerDeath
    /// </summary>
    void OnPlayerDeath()
    {
        if (race == Race.Player)
        {
            GameManagement.Instance.isPlayerDeath = true;
            GameUI.Instance.challengeBoss_Button.gameObject.SetActive(false);
            GameUI.Instance.bossUI_Transform.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// EnemyDeath
    /// </summary>
    void OnEnemyDeath()
    {
        if (race == Race.Enemy)
        {
            GameDataManagement.Instance.playerExperience += NumericalValueManagement.NumericalValue_Game.enemyExperience;
            GameDataManagement.Instance.playerGold += NumericalValueManagement.NumericalValue_Game.enemyBonus;

            if (GameDataManagement.Instance.playerExperience >=
                ((GameDataManagement.Instance.playerGrade - 1) * NumericalValueManagement.NumericalValue_Game.raiseUpgradeExperience) + NumericalValueManagement.NumericalValue_Game.upgradeExperience)
            {
                OnPlayerUpgrade();
                GameDataManagement.Instance.playerExperience = 0;
            }

            GameUI.Instance.OnSetPlayerExperience();
            GameUI.Instance.OnSetPlayerGold();
            GameManagement.Instance.GetEnemyList.Remove(transform);
            GameDataManagement.Instance.OnSaveJsonData();
        }
    }

    /// <summary>
    /// BossDeath
    /// </summary>
    public void OnBossDeath()
    {
        //Boss
        if (GameManagement.Instance.isChallengeBoss && race == Race.Enemy)
        {                       
            GameManagement.Instance.isChallengeBoss = false;            
            GameManagement.Instance.OnCleanBoss("BossObject", AssetManagement.Instance.boss_List);
            GameManagement.Instance.GetPlayerObject.OnUpdateValue();            
            GameDataManagement.Instance.gameLevel++;
            GameDataManagement.Instance.selectBossType = -1;
            GameDataManagement.Instance.playerGold += NumericalValueManagement.NumericalValue_Game.bossBonus * GameDataManagement.Instance.gameLevel;
            GameUI.Instance.OnSetPlayerGold();
            GameUI.Instance.OnSetGameLevel();
            GameUI.Instance.challengeBoss_Button.gameObject.SetActive(true);
            GameUI.Instance.bossUI_Transform.gameObject.SetActive(false);            
        }
    }
    #endregion

    /// <summary>
    /// RecoverHp
    /// </summary>
    /// <param name="recoverValue"></param>
    public void OnRecoverHp(int recoverValue)
    {
        Hp += recoverValue;

        if (Hp >= MaxHp) Hp = MaxHp;

        //TextEffect
        GameManagement.Instance.OnCreateTextEffect(attacker: transform,
                                                   position: transform.position + (thisCollider.center * transform.localScale.x),
                                                   color: Color.green,
                                                   text: recoverValue.ToString(),
                                                   type: TextEffect.TextType.Up);

        //Effect
        GameManagement.Instance.OnCreateEffect_Generally(position: transform.position + (thisCollider.center * transform.localScale.x),
                                                         forward: transform.forward,
                                                         effectName: "RecoverHp");
    }

    /// <summary>
    /// PlayerUpgrade
    /// </summary>
    void OnPlayerUpgrade()
    {        
        GameDataManagement.Instance.playerGrade++;
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();
        GameManagement.Instance.OnCreateUpGradeEffect(GameManagement.Instance.GetPlayerObject.transform);
        GameManagement.Instance.OnCreateTextEffect(attacker: GameManagement.Instance.GetPlayerObject.transform,
                                                   position: GameManagement.Instance.GetPlayerObject.transform.position + Vector3.up * 1.5f,
                                                   color: Color.yellow,
                                                   text: "Level UP!",
                                                   type: TextEffect.TextType.Up);
        GameUI.Instance.OnSetPlayerGrade();
    }

    /// <summary>
    /// AnimationOver
    /// </summary>
    protected void OnAnimationOver()
    {
        if (info.normalizedTime >= 1)
        {
            if (info.IsTag("Attack")) animator.SetFloat("AttackNumber", 0);
            if (info.IsTag("Death"))
            {
                gameObject.SetActive(false);

                //Change Music
                if(GameManagement.Instance.isChallengeBoss)
                {
                    AudioManagement.Instance.audioSource.clip = AssetManagement.Instance.OnSearchAssets<AudioClip>("BackgroundMusic", AssetManagement.Instance.backgroundMusic);
                    AudioManagement.Instance.audioSource.Play();
                }

                OnBossDeath();
            }
        }
    }

    /// <summary>
    /// AnimatorStateInfo
    /// </summary>
    protected void OnAnimatorStateInfo()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (thisCollider.center * transform.localScale.x) , attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + (thisCollider.center * transform.localScale.x) + transform.forward * attackDistance, damageOverTimeRadius);*/
    }
}
