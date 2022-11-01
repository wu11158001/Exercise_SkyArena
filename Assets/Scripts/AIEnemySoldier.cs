using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemySoldier : AIPlayer
{
    /// <summary>
    /// CheckTarget
    /// </summary>
    protected override void OnCheckTarget()
    {        
        if (targetObject == null)
        {
            OnSearchTarget();
        }
    }

    /// <summary>
    /// SearchTarget
    /// </summary>
    public override void OnSearchTarget()
    {        
        targetObject = GameManagement.Instance.GetPlayerObject.transform;
    }

    /// <summary>
    /// InitialNumericalValue
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();

        race = Race.Enemy;  

        moveSpeed = NumericalValueManagement.NumericalValue_EnemySoldier.moveSpeed;
        attackCount = NumericalValueManagement.NumericalValue_EnemySoldier.attackCount;
        attackRadius = NumericalValueManagement.NumericalValue_EnemySoldier.attackRadius;
        attackFrequency = NumericalValueManagement.NumericalValue_EnemySoldier.attackFrequency;

        OnUpdateValue();
    }

    /// <summary>
    /// UpdateValue
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_EnemySoldier.initial_Hp + 
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));
        attackPower = NumericalValueManagement.NumericalValue_EnemySoldier.initial_AttackPower +
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));
    }
}
