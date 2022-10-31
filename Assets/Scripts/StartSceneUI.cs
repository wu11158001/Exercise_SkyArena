using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 開始場景UI
/// </summary>
public class StartSceneUI : MonoBehaviour
{
    [Header("背景")]
    [SerializeField] [Tooltip("背景物件")] Image backgroundObject;

    [Header("提示文字")]
    [SerializeField] [Tooltip("提示文字物件")] Text tipTextObject;
    [SerializeField] [Tooltip("提示文字Alpha")] float tipTextAlpha;
    [SerializeField] [Tooltip("提示文字閃爍值")] float tipTextFlickerValue;
    [SerializeField] [Tooltip("提示文字閃爍速度")] float tipTextFlickerSpeed;

    [Header("淡出")]
    [SerializeField] [Tooltip("淡出速度")] float fadeSpeed;
    [SerializeField] [Tooltip("提示文字淡出閃爍速度")] float tipTextFadeFlickerSpeed;

    [Header("判斷")]
    [SerializeField] [Tooltip("是否淡出場景")] bool isFadeScene;
    [SerializeField] [Tooltip("是否載入場景")] bool isLoadingScene;

    private void Start()
    {
        OnBackgroundSize();//背景Size
    }

    private void Update()
    {
        OnTipTextFlicker();//提示文字閃爍
        OnPlayerClick();//玩家點擊螢幕
        OnScreenFabe();//畫面淡出
    }

    /// <summary>
    /// 背景Size
    /// </summary>
    void OnBackgroundSize()
    {
        backgroundObject.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    void OnLoadingScene()
    {
        if (backgroundObject.color.r <= 0 && !isLoadingScene)
        {
            isLoadingScene = true;
            SceneManager.LoadScene("GameScene");
        }
    }

    /// <summary>
    /// 畫面淡出
    /// </summary>
    void OnScreenFabe()
    {
        if(isFadeScene)//是否淡出場景
        {
            //物件淡出
            backgroundObject.color = OnFadeObject(backgroundObject.color);//背景
            tipTextObject.color = OnFadeObject(tipTextObject.color);//提示文字

            OnLoadingScene();//載入場景
        }
    }

    /// <summary>
    /// 物件淡出
    /// </summary>
    /// <param name="objColor">淡出物件顏色</param>
    /// <returns></returns>
    Color OnFadeObject(Color objColor)
    {        
        Color color = objColor;
        color.r -= fadeSpeed * Time.deltaTime;
        color.g -= fadeSpeed * Time.deltaTime;
        color.b -= fadeSpeed * Time.deltaTime;        
        return color;
    }

    /// <summary>
    /// 玩家點擊螢幕
    /// </summary>
    void OnPlayerClick()
    {
        //點擊開始
        if (Input.anyKeyDown)
        {
            isFadeScene = true;//是否淡出場景        
            tipTextFlickerSpeed = tipTextFadeFlickerSpeed;//提示文字閃爍速度
        }
    }

    /// <summary>
    /// 提示文字閃爍
    /// </summary>
    void OnTipTextFlicker()
    {
        tipTextObject.text = "任意鍵開始";
        tipTextAlpha += tipTextFlickerValue * Time.deltaTime;
        if (tipTextAlpha <= 0) tipTextFlickerValue = tipTextFlickerSpeed;
        if (tipTextAlpha >= 1) tipTextFlickerValue = -tipTextFlickerSpeed;
        Color color = tipTextObject.color;
        color.a = tipTextAlpha;
        tipTextObject.color = color;
    }
}
