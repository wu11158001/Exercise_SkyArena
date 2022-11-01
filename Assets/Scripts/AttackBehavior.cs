using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior
{
    /// <summary>
    /// OnSingleAttack
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="attackerRace"></param>
    /// <param name="target"></param>
    /// <param name="attackPower"></param>
    /// <param name="effectName"></param>
    public void OnSingleAttack(Transform attacker , AIPlayer.Race attackerRace, Transform target, int attackPower, string effectName)
    {
        target.GetComponent<AIPlayer>().OnGetHit(attacker: attacker,
                                                 attackerRace: attackerRace,
                                                 attack: attackPower,
                                                 effectName: effectName);
    }

   
}
