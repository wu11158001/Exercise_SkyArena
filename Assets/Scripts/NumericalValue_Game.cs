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
        public static int enemyGold = 50;
        public static int upgradeExperience = 10;
        public static int raiseUpgradeExperience = 20;

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
        public static float effectMoveSpeed = 20;
    }

    /// <summary>
    /// Player
    /// </summary>
    public class NumericalValue_Player
    {
        public static float moveSpeed = 2.5f;
        public static int initial_Hp = 500;
        public static int initial_AttackPower = 50;
        public static int attackCount = 3;
        public static float attackRadius = 1.6f;
        public static float attackFrequency = 0.1f;
        public static int raiseUpgradeHp = 100;
        public static int raiseUpgradeAttack = 5;
    }

    /// <summary>
    /// EnemySoldier
    /// </summary>
    public class NumericalValue_EnemySoldier
    {
        public static float moveSpeed = 1f;
        public static int initial_Hp = 100;
        public static int initial_AttackPower = 5;
        public static int attackCount = 1;
        public static float attackRadius = 1.6f;
        public static float attackFrequency = 1;
        public static int raiseUpgradeHp = 50;
        public static int raiseUpgradeAttack = 5;
    }

    /// <summary>
    /// Boss
    /// </summary>
    public class NumericalValue_Boss
    {
        public static float moveSpeed = 2f;
        public static int initial_Hp = 1000;
        public static int initial_AttackPower = 70;
        public static int attackCount = 3;
        public static float attackRadius = 4.5f;
        public static float attackFrequency = 1;
        public static int raiseUpgradeHp = 500;
        public static int raiseUpgradeAttack = 30;
        public static float attackDistance = 2f;
        public static float damageOverTimeRadius = 1.0f;
    }

    /// <summary>
    /// PlayerSkill
    /// </summary>
    public class NumbericalValue_PlayerSkill
    {
        public static float[] playerSkillsCD = new float[] { 5, 8, 4, 5 };

        public static int[] initialSkillValue = new int[] { 10, 11, 12, 13 };
        public static int[] raiseSkillValue = new int[] { 5, 6, 7, 8 };

        [Tooltip("RecoverHp")]
        public static int initialRecoverHpValue = 10;
        public static int raiseRecoverHpValue = 5;

        [Tooltip("Tornado")]
        public static int initialTornadoDamage = 11;
        public static int raiseTornado = 5;

        [Tooltip("SkillInformation")]
        public static string[] skillInformation = new string[]
        {
            //Skill_1
            $"'Recover HP'\nRecover Hp: ",
            //Skill_2
            "'Tornado'\nDamage Over Time, Damage:",
            //Skill_3
            "Skill3:",
            //Skill_4
            "Skill4:"
        };
    }
}
