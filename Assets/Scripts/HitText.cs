using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������r
/// </summary>
public class HitText : MonoBehaviour
{
    [Tooltip("����")] Text thisText;
    [Tooltip("Canvas")] Canvas canvas;
    [Tooltip("�����̪���e��")] Vector3 attackerForward;
    [Tooltip("��l��m")] Vector3 initialPosition;
    [Tooltip("��m")] Vector3 textPosition;
    [Tooltip("��ܮɶ�")] float lifeTime;
    [Tooltip("��ܮɶ�(�p�ɾ�)")] float lifeTimeCountDown;    
    [Tooltip("�V�W���ʳt��")] float upSpeed;
    [Tooltip("�V�U���ʳt��")] float downSpeed;
    [Tooltip("�V�Ჾ�ʳt��")] float backSpeed;
    [Tooltip("�W�[���V�Ჾ�ʳt��")] float addBackSpeed;
    [Tooltip("�W�[���V�U���ʳt��")] float addDownSpeed;
    [Tooltip("���Alpha�t��")] float loseAlphaSpeed;
    [Tooltip("Alpha�t��")] float AlphaSpeed;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        canvas = GameObject.FindObjectOfType<Canvas>();

        thisText.fontSize = 40;//��r�j�p
        lifeTime = 0.75f;//��ܮɶ�        
        downSpeed = 4;//�V�U���ʳt��
        backSpeed = 0.5f;//�V�Ჾ�ʳt��
        AlphaSpeed = 3;//Alpha�t��
    }

    private void Update()
    {
        OnTextDehavior();//��r�欰
    }

    /// <summary>
    /// �]�w��r
    /// </summary>
    /// <param name="attacker">�����̪���e��</param>
    /// <param name="pos">��l��m</param>
    /// <param name="race">�Q�����̺ر�</param>
    /// <param name="text">��ܤ�r</param>
    public void OnSetText(Vector3 attackerForward, Vector3 pos, AIPlayer.Race race, string text)
    {
        thisText.enabled = true;
        transform.SetParent(canvas.transform);             
        
        thisText.color = race == AIPlayer.Race.Player ? Color.red : Color.white;
        upSpeed = 0;//�V�W���ʳt��
        addBackSpeed = 0;//�V�Ჾ�ʳt��
        lifeTimeCountDown = lifeTime;//��ܮɶ�(�p�ɾ�)        
        addDownSpeed = 0;//���s�W�[���V�U���ʳt��
        loseAlphaSpeed = 0;//���s���Alpha�t��

        this.attackerForward = attackerForward;//�����̪���e��
        initialPosition = pos;//��l��m
        thisText.text = text;//��ܤ�r        
    }

    /// <summary>
    /// ��r�欰
    /// </summary>
    void OnTextDehavior()
    {
        if (lifeTimeCountDown <= 0) return;

        lifeTimeCountDown -= Time.deltaTime;
        if(lifeTimeCountDown <= 0)
        {
            gameObject.SetActive(false);
        }

        //��r����        
        if (lifeTimeCountDown / lifeTime > 0.85f)
        {
            upSpeed += Time.deltaTime;            
        }
        else
        {
            addDownSpeed += downSpeed * Time.deltaTime;
            loseAlphaSpeed += AlphaSpeed * Time.deltaTime;
            upSpeed -= addDownSpeed * Time.deltaTime;          

            thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, 1 - loseAlphaSpeed);
        }
        addBackSpeed += backSpeed * Time.deltaTime;
        textPosition = initialPosition+ Vector3.up * upSpeed + attackerForward * addBackSpeed;

        //�P�_Canvas�Ҧ�
        Camera camera = canvas.worldCamera;
        Vector3 position = Camera.main.WorldToScreenPoint(textPosition);        
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || camera == null)
        {
            transform.position = position;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, camera, out Vector2 localPosition);
        }
    }
}
