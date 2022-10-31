using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NumericalValueManagement
{
    /// <summary>
    /// Game
    /// </summary>
    public class NumericalValue_Game 
    {
        public static float deathTime = 3;
        public static float[] createEnemyTime = new float[] { 0.5f, 2 };
        public static int enemyExperience = 10;
        public static int upgradeExperience = 10;
        public static int raiseUpgradeExperience = 20;
    }

    /// <summary>
    /// Commom
    /// </summary>
    public class NumericalValue_Commom
    {        
        public static float commomValue_RotateSpeed = 0.1f;   
    }

    /// <summary>
    /// Player
    /// </summary>
    public class NumericalValue_Player
    {
        public static float moveSpeed = 2f;
        public static int initial_Hp = 500;
        public static int initial_Attack = 50;
        public static int attackCount = 3;
        public static float attackRadius = 1.2f;
        public static float attackFrequency = 0.1f;
        public static int raiseUpgradeHp = 100;
        public static int raiseUpgradeAttack = 70;
    }

    /// <summary>
    /// EnemySoldier
    /// </summary>
    public class NumericalValue_EnemySoldier
    {
        public static float moveSpeed = 1f;
        public static int initial_Hp = 100;
        public static int initial_Attack = 5;
        public static int attackCount = 1;
        public static float attackRadius = 1.2f;
        public static float attackFrequency = 1;
        public static int raiseUpgradeHp = 50;
        public static int raiseUpgradeAttack = 5;
    }

    /// <summary>
    /// Boss
    /// </summary>
    public class NumericalValue_Boss
    {
        public static float moveSpeed = 1f;
        public static int initial_Hp = 500;
        public static int initial_Attack = 70;
        public static int attackCount = 3;
        public static float attackRadius = 5;
        public static float attackFrequency = 1;
        public static int raiseUpgradeHp = 500;
        public static int raiseUpgradeAttack = 70;
    }
}
