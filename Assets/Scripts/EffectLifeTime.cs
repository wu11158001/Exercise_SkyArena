using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效生存時間
/// </summary>
public class EffectLifeTime : MonoBehaviour
{
    [Tooltip("生存時間")] float lifeTime;
    [Tooltip("生存時間(計時器)")] float lifeTimeCountDown;

    private void Awake()
    {
        lifeTime = 1;//生存時間
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
