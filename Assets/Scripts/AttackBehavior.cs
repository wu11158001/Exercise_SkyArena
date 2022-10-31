using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻擊行為
/// </summary>
public class AttackBehavior
{
    /// <summary>
    /// 單體攻擊
    /// </summary>
    /// <param name="attacker">攻擊者物件</param>
    /// <param name="attackerRace">攻擊者種族</param>
    /// <param name="target">攻擊目標</param>
    /// <param name="attack">攻擊力</param>
    /// <param name="effectName">特效名稱</param>
    public void OnSingleAttack(Transform attacker , AIPlayer.Race attackerRace, Transform target, int attack, string effectName)
    {
        target.GetComponent<AIPlayer>().OnGetHit(attacker: attacker,//攻擊者物件
                                                 attackerRace: attackerRace,//攻擊者種族
                                                 attack: attack,//攻擊力
                                                 effectName: effectName);//特效名稱
    }
}
