using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCollisionAttack : MonoBehaviour
{
    [Tooltip("Attacker")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;
    [Tooltip("AttackPower")] public int attackPower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnGetHit(attacker: attacker,
                                                                               attackerRace: attackerRace,
                                                                               attack: attackPower,
                                                                               effectName: "BasicGetHit_1");
    }
}
