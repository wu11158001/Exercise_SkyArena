using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 擊中文字
/// </summary>
public class HitText : MonoBehaviour
{
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
        OnTextDehavior();
    }

    /// <summary>
    /// SetText
    /// </summary>
    /// <param name="attackerForward"></param>
    /// <param name="pos"></param>
    /// <param name="race"></param>
    /// <param name="text"></param>
    public void OnSetText(Vector3 attackerForward, Vector3 pos, AIPlayer.Race race, string text)
    {
        thisText.enabled = true;
        transform.SetParent(canvas.transform);             
        
        thisText.color = race == AIPlayer.Race.Player ? Color.red : Color.white;
        upSpeed = 0;
        addBackSpeed = 0;
        lifeTimeCountDown = lifeTime;
        addDownSpeed = 0;
        loseAlphaSpeed = 0;

        this.attackerForward = attackerForward;
        initialPosition = pos;
        thisText.text = text;
    }

    /// <summary>
    /// TextDehavior
    /// </summary>
    void OnTextDehavior()
    {
        if (lifeTimeCountDown <= 0) return;

        lifeTimeCountDown -= Time.deltaTime;
        if(lifeTimeCountDown <= 0)
        {
            gameObject.SetActive(false);
        }

        //文字移動        
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
}
