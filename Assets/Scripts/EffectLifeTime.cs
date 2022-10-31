using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S�ĥͦs�ɶ�
/// </summary>
public class EffectLifeTime : MonoBehaviour
{
    [Tooltip("�ͦs�ɶ�")] float lifeTime;
    [Tooltip("�ͦs�ɶ�(�p�ɾ�)")] float lifeTimeCountDown;

    private void Awake()
    {
        lifeTime = 1;//�ͦs�ɶ�
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
