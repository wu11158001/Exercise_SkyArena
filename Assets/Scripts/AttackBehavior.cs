using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior
{    
    [Tooltip("TimeCountDown")] float timeCountDown;
    [Tooltip("DamageTime")] const float damageTime = 0.2f;
    [Tooltip("Target")] public Transform target;
    [Tooltip("Target")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;
    [Tooltip("AttackPower")] public int attackPower;    
    [Tooltip("DamageOverTimeRadius")] public float damageOverTimeRadius;

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
        if(target.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnGetHit(attacker: attacker,
                                                                                      attackerRace: attackerRace,
                                                                                      attack: attackPower,
                                                                                      effectName: effectName);
    }    
}
