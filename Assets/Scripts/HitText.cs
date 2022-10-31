using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 擊中文字
/// </summary>
public class HitText : MonoBehaviour
{
    [Tooltip("物件")] Text thisText;
    [Tooltip("Canvas")] Canvas canvas;
    [Tooltip("攻擊者物件前方")] Vector3 attackerForward;
    [Tooltip("初始位置")] Vector3 initialPosition;
    [Tooltip("位置")] Vector3 textPosition;
    [Tooltip("顯示時間")] float lifeTime;
    [Tooltip("顯示時間(計時器)")] float lifeTimeCountDown;    
    [Tooltip("向上移動速度")] float upSpeed;
    [Tooltip("向下移動速度")] float downSpeed;
    [Tooltip("向後移動速度")] float backSpeed;
    [Tooltip("增加的向後移動速度")] float addBackSpeed;
    [Tooltip("增加的向下移動速度")] float addDownSpeed;
    [Tooltip("減少Alpha速度")] float loseAlphaSpeed;
    [Tooltip("Alpha速度")] float AlphaSpeed;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        canvas = GameObject.FindObjectOfType<Canvas>();

        thisText.fontSize = 40;//文字大小
        lifeTime = 0.75f;//顯示時間        
        downSpeed = 4;//向下移動速度
        backSpeed = 0.5f;//向後移動速度
        AlphaSpeed = 3;//Alpha速度
    }

    private void Update()
    {
        OnTextDehavior();//文字行為
    }

    /// <summary>
    /// 設定文字
    /// </summary>
    /// <param name="attacker">攻擊者物件前方</param>
    /// <param name="pos">初始位置</param>
    /// <param name="race">被攻擊者種族</param>
    /// <param name="text">顯示文字</param>
    public void OnSetText(Vector3 attackerForward, Vector3 pos, AIPlayer.Race race, string text)
    {
        thisText.enabled = true;
        transform.SetParent(canvas.transform);             
        
        thisText.color = race == AIPlayer.Race.Player ? Color.red : Color.white;
        upSpeed = 0;//向上移動速度
        addBackSpeed = 0;//向後移動速度
        lifeTimeCountDown = lifeTime;//顯示時間(計時器)        
        addDownSpeed = 0;//重製增加的向下移動速度
        loseAlphaSpeed = 0;//重製減少Alpha速度

        this.attackerForward = attackerForward;//攻擊者物件前方
        initialPosition = pos;//初始位置
        thisText.text = text;//顯示文字        
    }

    /// <summary>
    /// 文字行為
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

        //判斷Canvas模式
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
