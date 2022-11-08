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
        public static float[] createEnemyTime = new float[] { 0.5f, 2.0f };
        public static int enemyExperience = 10;
        public static int enemyGold = 50;
        public static int upgradeExperience = 10;
        public static int raiseUpgradeExperience = 20;
        public static float attackFrequency = 0.2f;
        public static int bossBonus = 1000;

        //AFK
        public static int afkRewardTiming = 5;
        public static int akfExperienceReward = 1;
        public static int akfGoldReward = 10;

        //SkillCost
        public static int skillinItialCost = 500;
        public static int skillRaiseCost = 600;
    }

    /// <summary>
    /// Commom
    /// </summary>
    public class NumericalValue_Commom
    {        
        public static float commomValue_RotateSpeed = 0.1f;
        public static float effectTrackMoveSpeed = 20;
        public static float effectMoveForwardSpeed = 8;
    }

    /// <summary>
    /// Player
    /// </summary>
    public class NumericalValue_Player
    {
        public static float moveSpeed = 2.5f;
        public static int initial_Hp = 500;
        public static int raiseUpgradeHp = 100;
        public static int initial_AttackPower = 50;
        public static int raiseUpgradeAttack = 5;
        public static int attackCount = 3;
        public static float attackRadius = 1.6f;
        public static float attackFrequency = 0.1f;               
    }

    /// <summary>
    /// EnemySoldier
    /// </summary>
    public class NumericalValue_EnemySoldier
    {
        public static float moveSpeed = 1f;
        public static int initial_Hp = 100;
        public static int raiseUpgradeHp = 50;
        public static int initial_AttackPower = 5;
        public static int raiseUpgradeAttack = 5;
        public static int attackCount = 1;
        public static float attackRadius = 1.6f;
        public static float attackFrequency = 1;
    }

    /// <summary>
    /// Boss
    /// </summary>
    public class NumericalValue_Boss
    {
        public static float moveSpeed = 2f;
        public static int initial_Hp = 1000;
        public static int raiseUpgradeHp = 600;
        public static int initial_AttackPower = 70;
        public static int raiseUpgradeAttack = 120;
        public static int attackCount = 3;
        public static float attackRadius = 4.5f;
        public static float attackFrequency = 1;                
        public static float attackDistance = 2f;
        public static float damageOverTimeRadius = 1.0f;
    }

    /// <summary>
    /// PlayerSkill
    /// </summary>
    public class NumbericalValue_PlayerSkill
    {
        public static float[] playerSkillsCD = new float[] { 6.0f, 5.0f, 4.0f, 4.5f, 3.3f };
        public static int[] initialSkillValue = new int[] { 100, 20, 120, 40, 150 };
        public static int[] raiseSkillValue = new int[] { 48, 9, 19, 8, 27 };

        [Tooltip("SkillInformation")]
        public static string[] skillInformation = new string[]
        {
            //Skill_1
            $"\n'Recover HP'\nRecover Hp: ",
            //Skill_2
            "\n'Tornado'\nDamage over time, damage: ",
            //Skill_3
            "\n'LightningBall'\n:Bounce attack 3 time, Damage: ",
            //Skill_4
            "\n'Splash'\nRandom attack enemy 5 time, Damage: ",
            //Skill_5
            "\n'Slicer'\nFire the cutter forward, Damage: "
        };
    }
}
