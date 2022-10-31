using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵人士兵AI
/// </summary>
public class AIEnemySoldier : AIPlayer
{
    /// <summary>
    /// 檢查目標是否存在
    /// </summary>
    protected override void OnCheckTarget()
    {        
        if (targetObject == null)
        {
            OnSearchTarget();//搜尋攻擊目標
        }
    }

    /// <summary>
    /// 搜尋攻擊目標
    /// </summary>
    public override void OnSearchTarget()
    {        
        targetObject = GameManagement.Instance.GetPlayerObject.transform;
    }

    /// <summary>
    /// 初始數值
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();

        race = Race.Enemy;//種族              

        moveSpeed = NumericalValueManagement.NumericalValue_EnemySoldier.moveSpeed;//移動速度
        attackCount = NumericalValueManagement.NumericalValue_EnemySoldier.attackCount;//可使用攻擊招式數量
        attackRadius = NumericalValueManagement.NumericalValue_EnemySoldier.attackRadius;//攻擊半徑
        attackFrequency = NumericalValueManagement.NumericalValue_EnemySoldier.attackFrequency;//攻擊頻率

        OnUpdateValue();//更新數值
    }

    /// <summary>
    /// 更新數值
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_EnemySoldier.initial_Hp + 
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));//生命值
        attack = NumericalValueManagement.NumericalValue_EnemySoldier.initial_Attack +
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));//攻擊力
    }
}
