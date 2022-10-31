using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] [Tooltip("­backgroundObject")] Image backgroundObject;

    [Header("TipText")]
    [SerializeField] [Tooltip("TipTextObject")] Text tipTextObject;
    [SerializeField] [Tooltip("TipTextAlpha")] float tipTextAlpha;
    [SerializeField] [Tooltip("TipTextFlickerValue")] float tipTextFlickerValue;
    [SerializeField] [Tooltip("TipTextFlickerSpeed")] float tipTextFlickerSpeed;

    [Header("TipText ")]
    [SerializeField] [Tooltip("FadeSpeed")] float fadeSpeed;
    [SerializeField] [Tooltip("TipTextFadeFlickerSpeed")] float tipTextFadeFlickerSpeed;

    [Header("Judge")]
    [SerializeField] [Tooltip("IsFadeScene")] bool isFadeScene;
    [SerializeField] [Tooltip("IsLoadingScene")] bool isLoadingScene;

    private void Start()
    {
        OnBackgroundSize();
    }

    private void Update()
    {
        OnTipTextFlicker();
        OnPlayerClick();
        OnScreenFabe();
    }

    /// <summary>
    /// ­BackgroundSize
    /// </summary>
    void OnBackgroundSize()
    {
        backgroundObject.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    /// <summary>
    /// LoadingScene
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
    /// ScreenFabe
    /// </summary>
    void OnScreenFabe()
    {
        if(isFadeScene)
        {
            backgroundObject.color = OnFadeObject(backgroundObject.color);
            tipTextObject.color = OnFadeObject(tipTextObject.color);

            OnLoadingScene();
        }
    }

    /// <summary>
    /// FadeObject
    /// </summary>
    /// <param name="objColor"></param>
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
    /// PlayerClick
    /// </summary>
    void OnPlayerClick()
    {  
        if (Input.anyKeyDown)
        {
            isFadeScene = true;
            tipTextFlickerSpeed = tipTextFadeFlickerSpeed;
        }
    }

    /// <summary>
    /// TipTextFlicker
    /// </summary>
    void OnTipTextFlicker()
    {
        tipTextObject.text = "Any Key Start";
        tipTextAlpha += tipTextFlickerValue * Time.deltaTime;
        if (tipTextAlpha <= 0) tipTextFlickerValue = tipTextFlickerSpeed;
        if (tipTextAlpha >= 1) tipTextFlickerValue = -tipTextFlickerSpeed;
        Color color = tipTextObject.color;
        color.a = tipTextAlpha;
        tipTextObject.color = color;
    }
}
