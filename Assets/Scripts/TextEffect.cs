using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    public enum TextType { GoBack, Up }
    TextType textType;

    [Tooltip("ThisText")] Text thisText;
    [Tooltip("Canvas")] Canvas canvas;

    [Tooltip("InitialTextSize")] const int initialTextSize = 40;
    [Tooltip("AttackerForward")] Vector3 attackerForward;
    [Tooltip("InitialPosition")] Vector3 initialPosition;
    [Tooltip("TextPosition")] Vector3 textPosition;

    [Tooltip("LifeTime")] const float lifeTime = 0.75f;
    [Tooltip("LifeTimeCountDown")] float lifeTimeCountDown;

    [Tooltip("UpSpeed")] float upSpeed;
    [Tooltip("DownSpeed")] const float downSpeed = 4;
    [Tooltip("BackSpeed")] const float backSpeed = 0.5f;
    [Tooltip("AddBackSpeed")] float addBackSpeed;
    [Tooltip("AddDownSpeed")] float addDownSpeed;
    [Tooltip("LoseAlphaSpeed")] float loseAlphaSpeed;
    [Tooltip("AlphaSpeed")] const float alphaSpeed = 3;

    [Header("FontSize")]
    [Tooltip("InitialCameraDistance")] const float initialCameraDistance = 6.0f;
    [Tooltip("SizeChangeValue")] const float sizeChangeValue = 7;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        canvas = GameObject.FindObjectOfType<Canvas>();

        thisText.fontSize = initialTextSize;
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
        if (GameUI.Instance.isOpenInterface) gameObject.SetActive(false); ;

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
        //Size
        float cameraDistance = (CameraControl.Instance.gameObject.transform.position - GameManagement.Instance.GetPlayerObject.transform.position).magnitude;        
        thisText.fontSize = initialTextSize + (int)((initialCameraDistance - cameraDistance) * sizeChangeValue);

        //Life Time
        lifeTimeCountDown -= Time.deltaTime;
        if (lifeTimeCountDown <= 0) gameObject.SetActive(false); ;        

        //Text Move               
        if (textType == TextType.GoBack) OnGetHitTextBehavior();
        if (textType == TextType.Up) OnUpGradeTextBehavior();

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
        if (textType == TextType.GoBack)
        {
            if (lifeTimeCountDown / lifeTime > 0.85f)
            {
                upSpeed += Time.deltaTime;
            }
            else
            {
                addDownSpeed += downSpeed * Time.deltaTime;
                loseAlphaSpeed += alphaSpeed * Time.deltaTime;
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
        if(textType == TextType.Up)
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
