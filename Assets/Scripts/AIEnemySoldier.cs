using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH�h�LAI
/// </summary>
public class AIEnemySoldier : AIPlayer
{
    /// <summary>
    /// �ˬd�ؼЬO�_�s�b
    /// </summary>
    protected override void OnCheckTarget()
    {        
        if (targetObject == null)
        {
            OnSearchTarget();//�j�M�����ؼ�
        }
    }

    /// <summary>
    /// �j�M�����ؼ�
    /// </summary>
    public override void OnSearchTarget()
    {        
        targetObject = GameManagement.Instance.GetPlayerObject.transform;
    }

    /// <summary>
    /// ��l�ƭ�
    /// </summary>   
    public override void OnInitialNumericalValue()
    {
        base.OnInitialNumericalValue();

        race = Race.Enemy;//�ر�              

        moveSpeed = NumericalValueManagement.NumericalValue_EnemySoldier.moveSpeed;//���ʳt��
        attackCount = NumericalValueManagement.NumericalValue_EnemySoldier.attackCount;//�i�ϥΧ����ۦ��ƶq
        attackRadius = NumericalValueManagement.NumericalValue_EnemySoldier.attackRadius;//�����b�|
        attackFrequency = NumericalValueManagement.NumericalValue_EnemySoldier.attackFrequency;//�����W�v

        OnUpdateValue();//��s�ƭ�
    }

    /// <summary>
    /// ��s�ƭ�
    /// </summary>
    public override void OnUpdateValue()
    {
        Hp = NumericalValueManagement.NumericalValue_EnemySoldier.initial_Hp + 
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));//�ͩR��
        attack = NumericalValueManagement.NumericalValue_EnemySoldier.initial_Attack +
            (NumericalValueManagement.NumericalValue_EnemySoldier.raiseUpgradeAttack * (GameDataManagement.Instance.gameLevel - 1));//�����O
    }
}
