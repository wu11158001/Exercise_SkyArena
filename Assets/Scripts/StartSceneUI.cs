using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �}�l����UI
/// </summary>
public class StartSceneUI : MonoBehaviour
{
    [Header("�I��")]
    [SerializeField] [Tooltip("�I������")] Image backgroundObject;

    [Header("���ܤ�r")]
    [SerializeField] [Tooltip("���ܤ�r����")] Text tipTextObject;
    [SerializeField] [Tooltip("���ܤ�rAlpha")] float tipTextAlpha;
    [SerializeField] [Tooltip("���ܤ�r�{�{��")] float tipTextFlickerValue;
    [SerializeField] [Tooltip("���ܤ�r�{�{�t��")] float tipTextFlickerSpeed;

    [Header("�H�X")]
    [SerializeField] [Tooltip("�H�X�t��")] float fadeSpeed;
    [SerializeField] [Tooltip("���ܤ�r�H�X�{�{�t��")] float tipTextFadeFlickerSpeed;

    [Header("�P�_")]
    [SerializeField] [Tooltip("�O�_�H�X����")] bool isFadeScene;
    [SerializeField] [Tooltip("�O�_���J����")] bool isLoadingScene;

    private void Start()
    {
        OnBackgroundSize();//�I��Size
    }

    private void Update()
    {
        OnTipTextFlicker();//���ܤ�r�{�{
        OnPlayerClick();//���a�I���ù�
        OnScreenFabe();//�e���H�X
    }

    /// <summary>
    /// �I��Size
    /// </summary>
    void OnBackgroundSize()
    {
        backgroundObject.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    /// <summary>
    /// ���J����
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
    /// �e���H�X
    /// </summary>
    void OnScreenFabe()
    {
        if(isFadeScene)//�O�_�H�X����
        {
            //����H�X
            backgroundObject.color = OnFadeObject(backgroundObject.color);//�I��
            tipTextObject.color = OnFadeObject(tipTextObject.color);//���ܤ�r

            OnLoadingScene();//���J����
        }
    }

    /// <summary>
    /// ����H�X
    /// </summary>
    /// <param name="objColor">�H�X�����C��</param>
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
    /// ���a�I���ù�
    /// </summary>
    void OnPlayerClick()
    {
        //�I���}�l
        if (Input.anyKeyDown)
        {
            isFadeScene = true;//�O�_�H�X����        
            tipTextFlickerSpeed = tipTextFadeFlickerSpeed;//���ܤ�r�{�{�t��
        }
    }

    /// <summary>
    /// ���ܤ�r�{�{
    /// </summary>
    void OnTipTextFlicker()
    {
        tipTextObject.text = "���N��}�l";
        tipTextAlpha += tipTextFlickerValue * Time.deltaTime;
        if (tipTextAlpha <= 0) tipTextFlickerValue = tipTextFlickerSpeed;
        if (tipTextAlpha >= 1) tipTextFlickerValue = -tipTextFlickerSpeed;
        Color color = tipTextObject.color;
        color.a = tipTextAlpha;
        tipTextObject.color = color;
    }
}
