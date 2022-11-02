using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [Tooltip("LifeTime")] public float lifeTime;
    [Tooltip("LifeTimeCountDown")] public float lifeTimeCountDown;

    private void Awake()
    {
        lifeTime = 2f;
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
