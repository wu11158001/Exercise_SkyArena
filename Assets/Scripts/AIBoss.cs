using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoss : AIEnemySoldier
{
    /// <summary>
    /// InitialNumericalValue
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();
        
        race = Race.Enemy;       

        moveSpeed = NumericalValueManagement.NumericalValue_Boss.moveSpeed;
        attackCount = NumericalValueManagement.NumericalValue_Boss.attackCount;      
        attackRadius = NumericalValueManagement.NumericalValue_Boss.attackRadius;
        attackFrequency = NumericalValueManagement.NumericalValue_Boss.attackFrequency;

        //BossExclusive
        attackDistance = NumericalValueManagement.NumericalValue_Boss.attackDistance;
        damageOverTimeRadius = NumericalValueManagement.NumericalValue_Boss.damageOverTimeRadius;

        OnUpdateValue();
    }

    /// <summary>
    /// UpdateValue
    /// </summary>
    public override void OnUpdateValue()
    {
        MaxHp = NumericalValueManagement.NumericalValue_Boss.initial_Hp +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));

        Hp = MaxHp;

        attackPower = NumericalValueManagement.NumericalValue_Boss.initial_AttackPower +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));
    }
}
