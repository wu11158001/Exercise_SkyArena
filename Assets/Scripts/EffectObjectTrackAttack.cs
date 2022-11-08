using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectTrackAttack : MonoBehaviour
{
    [Tooltip("Target")] public Transform target;
    [Tooltip("RotateSpeed")] public float rotateSpeed;
    [Tooltip("RaiseRotateSpeed")] public float raiseRotateSpeed;
    [Tooltip("Attacker")] public Transform attacker;
    [Tooltip("Race")] public AIPlayer.Race attackerRace;
    [Tooltip("AttackPower")] public int attackPower;


    private void Update()
    {
        //RotateSpeed
        raiseRotateSpeed += 0.1f * Time.deltaTime;
        rotateSpeed += raiseRotateSpeed * Time.deltaTime;

        if (target != null)
        {
            transform.position = transform.position + transform.forward * NumericalValueManagement.NumericalValue_Commom.effectTrackMoveSpeed * Time.deltaTime;
            transform.forward = Vector3.RotateTowards(transform.forward, target.position - transform.position, rotateSpeed, 0);
        }
        else gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            if (other.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer))
            {
                aIPlayer.OnGetHit(attacker: attacker,
                                  attackerRace: attackerRace,
                                  attack: attackPower,
                                  effectName: "TrackAttackExplosions");

                gameObject.SetActive(false);
            }
        }
    }
}
