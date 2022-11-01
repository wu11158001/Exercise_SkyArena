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

        OnUpdateValue();//§ó·s¼Æ­È
    }

    /// <summary>
    /// UpdateValue
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_Boss.initial_Hp +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));
        attackPower = NumericalValueManagement.NumericalValue_Boss.initial_AttackPower +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));
    }
}
