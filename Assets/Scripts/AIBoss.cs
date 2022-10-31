using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BossAI
/// </summary>
public class AIBoss : AIEnemySoldier
{
    /// <summary>
    /// ��l�ƭ�
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();
        
        race = Race.Enemy;//�ر�        

        moveSpeed = NumericalValueManagement.NumericalValue_Boss.moveSpeed;//���ʳt��
        attackCount = NumericalValueManagement.NumericalValue_Boss.attackCount;//�i�ϥΧ����ۦ��ƶq        
        attackRadius = NumericalValueManagement.NumericalValue_Boss.attackRadius;//�����b�|
        attackFrequency = NumericalValueManagement.NumericalValue_Boss.attackFrequency;//�����W�v

        OnUpdateValue();//��s�ƭ�
    }

    /// <summary>
    /// ��s�ƭ�
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_Boss.initial_Hp +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));//�ͩR��
        attack = NumericalValueManagement.NumericalValue_Boss.initial_Attack +
            (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));//�����O
    }
}
