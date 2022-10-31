using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����欰
/// </summary>
public class AttackBehavior
{
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="attacker">�����̪���</param>
    /// <param name="attackerRace">�����̺ر�</param>
    /// <param name="target">�����ؼ�</param>
    /// <param name="attack">�����O</param>
    /// <param name="effectName">�S�ĦW��</param>
    public void OnSingleAttack(Transform attacker , AIPlayer.Race attackerRace, Transform target, int attack, string effectName)
    {
        target.GetComponent<AIPlayer>().OnGetHit(attacker: attacker,//�����̪���
                                                 attackerRace: attackerRace,//�����̺ر�
                                                 attack: attack,//�����O
                                                 effectName: effectName);//�S�ĦW��
    }
}
