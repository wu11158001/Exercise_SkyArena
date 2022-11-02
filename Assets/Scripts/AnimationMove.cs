using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMove : StateMachineBehaviour
{
    [Tooltip("MoveSpeed")] float moveSpeed;
    [Tooltip("Object")] Transform obj;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        if (moveSpeed == 0)
        {
            if (animator.gameObject.TryGetComponent<AIPlayer>(out AIPlayer aIPlayer))
            {
                moveSpeed = aIPlayer.moveSpeed;        
            }
        }
        
        if(obj == null)
        {
            obj = animator.gameObject.transform;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        obj.position = obj.position + obj.forward * moveSpeed * Time.deltaTime;
    }
}
