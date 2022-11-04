using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    public enum TextType { GetHit, UpGrade}
    TextType textType;

    Text thisText;
    Canvas canvas;
    Vector3 attackerForward;
    Vector3 initialPosition;
    Vector3 textPosition;
    float lifeTime;
    float lifeTimeCountDown;    
    float upSpeed;
    float downSpeed;
    float backSpeed;
    float addBackSpeed;
    float addDownSpeed;
    float loseAlphaSpeed;
    float AlphaSpeed;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        canvas = GameObject.FindObjectOfType<Canvas>();

        thisText.fontSize = 40;
        lifeTime = 0.75f;
        downSpeed = 4;
        backSpeed = 0.5f;
        AlphaSpeed = 3;
    }

    private void Update()
    {
        OnTextBehavior();
    }

    /// <summary>
    /// SetText
    /// </summary>
    /// <param name="attackerForward"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="text"></param>
    public void OnSetText(Vector3 attackerForward, Vector3 position, Color color, string text, TextType type)
    {
        thisText.enabled = true;
        transform.SetParent(canvas.transform);

        //Initial Value
        upSpeed = 0;
        addBackSpeed = 0;
        lifeTimeCountDown = lifeTime;
        addDownSpeed = 0;
        loseAlphaSpeed = 0;

        //Set Value
        textType = type;
        thisText.color = color;
        this.attackerForward = attackerForward;
        initialPosition = position;
        thisText.text = text;
    }

    /// <summary>
    /// TextBehavior
    /// </summary>
    void OnTextBehavior()
    {
        if (lifeTimeCountDown <= 0) return;

        //Life Time
        lifeTimeCountDown -= Time.deltaTime;
        if(lifeTimeCountDown <= 0)
        {
            gameObject.SetActive(false);
        }

        //Text Move               
        if (textType == TextType.GetHit) OnGetHitTextBehavior();
        if (textType == TextType.UpGrade) OnUpGradeTextBehavior();

        //Judge Canvas
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

    /// <summary>
    /// GetHitTextBehavior
    /// </summary>
    void OnGetHitTextBehavior()
    {
        if (textType == TextType.GetHit)
        {
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
            textPosition = initialPosition + Vector3.up * upSpeed + attackerForward * addBackSpeed;
        }
    }

    /// <summary>
    /// UpGradeTextBehavior
    /// </summary>
    void OnUpGradeTextBehavior()
    {
        if(textType == TextType.UpGrade)
        {
            if (lifeTimeCountDown / lifeTime > 0.75f)
            {
                upSpeed += Time.deltaTime;
            }
            else
            {
                thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, 1 - loseAlphaSpeed);
            }
            textPosition = initialPosition + Vector3.up * upSpeed;
        }
    }
}
