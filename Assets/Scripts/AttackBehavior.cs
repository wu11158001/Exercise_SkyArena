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
    /// <param name="attack"></param>
    /// <param name="effectName"></param>
    public void OnSingleAttack(Transform attacker , AIPlayer.Race attackerRace, Transform target, int attack, string effectName)
    {
        target.GetComponent<AIPlayer>().OnGetHit(attacker: attacker,
                                                 attackerRace: attackerRace,
                                                 attack: attack,
                                                 effectName: effectName);
    }
}
