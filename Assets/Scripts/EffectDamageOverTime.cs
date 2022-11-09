using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamageOverTime : MonoBehaviour
{
    [Tooltip("TimeCountDown")] public float timeCountDown;    
    [Tooltip("Target")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;
    [Tooltip("AttackPower")] public int attackPower;

    private void OnTriggerStay(Collider other)
    {
        timeCountDown -= Time.deltaTime;
        if (timeCountDown <= 0)
        {
            timeCountDown = NumericalValueManagement.NumericalValue_Game.attackFrequency;            
            if (other.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnGetHit(attacker: attacker,
                                                                                          attackerRace: attackerRace,
                                                                                          attack: attackPower,
                                                                                          effectName: "BasicGetHit_1",
                                                                                          soundEffectName: "GetHit");
        }
    }
}
