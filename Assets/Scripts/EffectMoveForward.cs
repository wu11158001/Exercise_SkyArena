using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMoveForward : MonoBehaviour
{
    [Tooltip("Damage")] public int damage;
    [Tooltip("Attacker")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;    

    private void Update()
    {
        transform.position = transform.position + transform.forward * NumericalValueManagement.NumericalValue_Commom.effectMoveForwardSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer)) aIPlayer.OnGetHit(attacker: attacker,
                                                                               attackerRace: attackerRace,
                                                                               attack: damage,
                                                                               effectName: "BasicGetHit_1",
                                                                               soundEffectName: "GetHit");
    }
}
