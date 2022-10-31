using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƭȤ���
/// </summary>
namespace NumericalValueManagement
{
    /// <summary>
    /// �ƭȤ���
    /// </summary>
    public class NumericalValueManagement 
    {
        public static float deathTime = 3;//���`�ɶ�
        public static float[] createEnemyTime = new float[] { 0.5f, 2 };//���ͼĤH�ɶ�
        public static int enemyExperience = 10;//�ĤH�g���
        public static int upgradeExperience = 10;//�ɯŸg���
        public static int raiseUpgradeExperience = 20;//�C���Žդɸg���
    }

    /// <summary>
    /// �@�P�ƭ�
    /// </summary>
    public class NumericalValue_Commom
    {        
        public static float commomValue_RotateSpeed = 0.1f;//��V�t��        
    }

    /// <summary>
    /// ���a�ƭ�
    /// </summary>
    public class NumericalValue_Player
    {
        public static float moveSpeed = 2f;//���ʳt��
        public static int initial_Hp = 500;//��l�ͩR
        public static int initial_Attack = 50;//��l����        
        public static int attackCount = 3;//�i�ϥΧ����ۦ��ƶq
        public static float attackRadius = 1.2f;//�����b�|
        public static float attackFrequency = 0.1f;//�����W�v
        public static int raiseUpgradeHp = 100;//�C���Žդ�Hp
        public static int raiseUpgradeAttack = 70;//�C���Žդɧ���
    }

    /// <summary>
    /// �ĤH�h�L�ƭ�
    /// </summary>
    public class NumericalValue_EnemySoldier
    {
        public static float moveSpeed = 1f;//���ʳt��
        public static int initial_Hp = 100;//��l�ͩR
        public static int initial_Attack = 5;//��l����        
        public static int attackCount = 1;//�i�ϥΧ����ۦ��ƶq
        public static float attackRadius = 1.2f;//�����b�|
        public static float attackFrequency = 1;//�����W�v
        public static int raiseUpgradeHp = 50;//�C���Žդ�Hp
        public static int raiseUpgradeAttack = 5;//�C���Žդɧ���
    }

    /// <summary>
    /// Boss�ƭ�
    /// </summary>
    public class NumericalValue_Boss
    {
        public static float moveSpeed = 1f;//���ʳt��
        public static int initial_Hp = 500;//��l�ͩR
        public static int initial_Attack = 70;//��l����    
        public static int attackCount = 3;//�i�ϥΧ����ۦ��ƶq        
        public static float attackRadius = 5;//�����b�|
        public static float attackFrequency = 1;//�����W�v
        public static int raiseUpgradeHp = 500;//�C���Žդ�Hp
        public static int raiseUpgradeAttack = 70;//�C���Žդɧ���
    }
}
