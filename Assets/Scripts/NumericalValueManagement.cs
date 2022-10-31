using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 數值中心
/// </summary>
namespace NumericalValueManagement
{
    /// <summary>
    /// 數值中心
    /// </summary>
    public class NumericalValueManagement 
    {
        public static float deathTime = 3;//死亡時間
        public static float[] createEnemyTime = new float[] { 0.5f, 2 };//產生敵人時間
        public static int enemyExperience = 10;//敵人經驗值
        public static int upgradeExperience = 10;//升級經驗值
        public static int raiseUpgradeExperience = 20;//每等級調升經驗值
    }

    /// <summary>
    /// 共同數值
    /// </summary>
    public class NumericalValue_Commom
    {        
        public static float commomValue_RotateSpeed = 0.1f;//轉向速度        
    }

    /// <summary>
    /// 玩家數值
    /// </summary>
    public class NumericalValue_Player
    {
        public static float moveSpeed = 2f;//移動速度
        public static int initial_Hp = 500;//初始生命
        public static int initial_Attack = 50;//初始攻擊        
        public static int attackCount = 3;//可使用攻擊招式數量
        public static float attackRadius = 1.2f;//攻擊半徑
        public static float attackFrequency = 0.1f;//攻擊頻率
        public static int raiseUpgradeHp = 100;//每等級調升Hp
        public static int raiseUpgradeAttack = 70;//每等級調升攻擊
    }

    /// <summary>
    /// 敵人士兵數值
    /// </summary>
    public class NumericalValue_EnemySoldier
    {
        public static float moveSpeed = 1f;//移動速度
        public static int initial_Hp = 100;//初始生命
        public static int initial_Attack = 5;//初始攻擊        
        public static int attackCount = 1;//可使用攻擊招式數量
        public static float attackRadius = 1.2f;//攻擊半徑
        public static float attackFrequency = 1;//攻擊頻率
        public static int raiseUpgradeHp = 50;//每等級調升Hp
        public static int raiseUpgradeAttack = 5;//每等級調升攻擊
    }

    /// <summary>
    /// Boss數值
    /// </summary>
    public class NumericalValue_Boss
    {
        public static float moveSpeed = 1f;//移動速度
        public static int initial_Hp = 500;//初始生命
        public static int initial_Attack = 70;//初始攻擊    
        public static int attackCount = 3;//可使用攻擊招式數量        
        public static float attackRadius = 5;//攻擊半徑
        public static float attackFrequency = 1;//攻擊頻率
        public static int raiseUpgradeHp = 500;//每等級調升Hp
        public static int raiseUpgradeAttack = 70;//每等級調升攻擊
    }
}
