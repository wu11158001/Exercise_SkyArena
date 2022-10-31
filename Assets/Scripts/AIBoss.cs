using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BossAI
/// </summary>
public class AIBoss : AIEnemySoldier
{
    /// <summary>
    /// 初始數值
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();
        
        race = Race.Enemy;//種族        

        moveSpeed = NumericalValueManagement.NumericalValue_Boss.moveSpeed;//移動速度
        attackCount = NumericalValueManagement.NumericalValue_Boss.attackCount;//可使用攻擊招式數量        
        attackRadius = NumericalValueManagement.NumericalValue_Boss.attackRadius;//攻擊半徑
        attackFrequency = NumericalValueManagement.NumericalValue_Boss.attackFrequency;//攻擊頻率

        OnUpdateValue();//更新數值
    }

    /// <summary>
    /// 更新數值
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_Boss.initial_Hp +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));//生命值
        attack = NumericalValueManagement.NumericalValue_Boss.initial_Attack +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));//攻擊力
    }
}
