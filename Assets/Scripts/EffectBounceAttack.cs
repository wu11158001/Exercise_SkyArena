using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBounceAttack : MonoBehaviour
{
    [Tooltip("MoveSpeed")] float moveSpeed = 5;
    [SerializeField] [Tooltip("BounceCount")] int bounceCount;
    [Tooltip("MaxBounceCount")] const int maxBounceCount = 3;
    [SerializeField] [Tooltip("Target")] Transform targetObject;
    [Tooltip("Attacker")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;
    [Tooltip("AttackPower")] public int attackPower;
    [Tooltip("RecordTarget")] List<Transform> attackedTarget_List = new List<Transform>();
    [SerializeField] [Tooltip("AttackObjectNumber")] int attackObjectNumber;
    [SerializeField] [Tooltip("IsWaitionDisable")] bool isWaitionDisable;

    private void Update()
    {
        OnMoveMent();
    }

    /// <summary>
    /// MoveMent
    /// </summary>
    void OnMoveMent()
    {
        if (targetObject == null ||
            targetObject.gameObject.activeSelf == false ||
            targetObject.position == transform.position)
        {
            OnFimdTarget();            
        }
        else
        {
            transform.forward = targetObject.position - transform.position;
            transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// FimdTarget
    /// </summary>
    void OnFimdTarget()
    {        
        if (OnRemoveCondition()) return;
        
        Transform target = GameManagement.Instance.GetEnemyList[attackObjectNumber];
        attackObjectNumber++;
        if (target == null) return;        

        for (int i = 0; i < attackedTarget_List.Count; i++)
        {
            if (target == attackedTarget_List[i])
            {
                targetObject = null;
                OnFimdTarget();
                return;
            }
        }
        targetObject = target;        
    }

    /// <summary>
    /// RemoveCondition
    /// </summary>
    bool OnRemoveCondition()
    {
        if (!isWaitionDisable)
        {
            if (GameManagement.Instance.GetEnemyList.Count == 0 ||
                attackObjectNumber >= GameManagement.Instance.GetEnemyList.Count ||
                bounceCount >= maxBounceCount)
            {
                isWaitionDisable = true;
                StartCoroutine(IWaitDisable());
                return isWaitionDisable;
            }
        }

        return isWaitionDisable;
    }

    /// <summary>
    /// IWaitDisable
    /// </summary>
    /// <returns></returns>
    IEnumerator IWaitDisable()
    {        
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// InitialValue
    /// </summary>
    public void OnInitialValue()
    {
        isWaitionDisable = false;
        attackedTarget_List.Clear();
        attackObjectNumber = 0;
        bounceCount = 0;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetObject &&
            other.gameObject.activeSelf &&
            bounceCount < maxBounceCount &&
            other.transform != attacker)
        {
            if (other.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer))
            {                
                aIPlayer.OnGetHit(attacker: attacker,
                                  attackerRace: attackerRace,
                                  attack: attackPower,
                                  effectName: "BasicGetHit_1",
                                  soundEffectName: "GetHit");
                
                bounceCount++;
                attackedTarget_List.Add(other.transform);
                OnFimdTarget();                
            }
        }        
    }
}
