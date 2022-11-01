using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMovement : MonoBehaviour
{
    private void Update()
    {
        transform.position = transform.position + transform.forward * NumericalValueManagement.NumericalValue_Commom.effectMoveSpeed * Time.deltaTime;
    }
}
