using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [Tooltip("LifeTime")] float lifeTime;
    [Tooltip("LifeTimeCountDown")] float lifeTimeCountDown;

    private void Awake()
    {
        lifeTime = 3;
    }

    private void Update()
    {
        lifeTimeCountDown += Time.deltaTime;
        if (lifeTimeCountDown >= lifeTime)
        {
            lifeTimeCountDown = 0;
            gameObject.SetActive(false);            
        }
    }    
}
