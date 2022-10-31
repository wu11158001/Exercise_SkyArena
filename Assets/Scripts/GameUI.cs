using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// �C��UI
/// </summary>
public class GameUI : MonoBehaviour
{
    static GameUI gameUI;
    public static GameUI Instance => gameUI;

    [Header("�e���H�X/�J")]
    [SerializeField] [Tooltip("�H�X/�J�Ϥ�Alpha")] float fadeAlpha;
    [SerializeField] [Tooltip("�H�X/�J�t��")] float fadeSpeed;
    [SerializeField] [Tooltip("���`Alpha")] float deathAlpha;
    [Tooltip("�H�X/�J�Ϥ�����")] Image fade_Image;

    [Header("���ܤ�r")]
    [SerializeField] [Tooltip("���`�ɶ�(�p�ɾ�)")] float deathTimeCountDown;
    [Tooltip("���ܤ�r")] Text tip_Text;

    [Header("���a��T")]
    [SerializeField] [Tooltip("���a����")] Text playerLevel_Text;
    [SerializeField] [Tooltip("���a���")] Image playerLifeBar_Image;
    [SerializeField] [Tooltip("���a�����r")] Text playerLifeBar_Text;
    [SerializeField] [Tooltip("���a�g���")] Image playerExperienceBar_Image;
    [SerializeField] [Tooltip("���a�g�����r")] Text playerExperienceBar_Text;
    [SerializeField] [Tooltip("���d���Ť�r")] Text gameLevel_Text;

    [Header("�D��Boss")]
    [SerializeField] [Tooltip("�D��Boss���s")] Button challengeBoss_Button;
    [SerializeField] [Tooltip("BossUI��ܱ���")] Transform bossUI_Transform;
    [SerializeField] [Tooltip("Boss���")] Image bossLifeBar;
    [SerializeField] [Tooltip("Boss�����r")] Text bossLifeBar_Text;


    private void Awake()
    {
        if (gameUI != null)
        {
            Destroy(this);
            return;
        }
        gameUI = this;
    }

    private void Start()
    {
        OnGetComponent();//���Component

        //�e���H�X/�J
        fadeAlpha = 1;//�H�X/�J�Ϥ�Alpha
        fade_Image.color = new Color(0, 0, 0, fadeAlpha);
        fadeSpeed = 0.3f;//�H�X/�J�t��
        deathAlpha = 0.85f;//���`Alpha

        //���ܤ�r
        deathTimeCountDown = NumericalValueManagement.NumericalValueManagement.deathTime;//���`�ɶ�(�p�ɾ�)
    }

    /// <summary>
    /// ���Component
    /// </summary>
    void OnGetComponent()
    {
        //�H�X/�J�Ϥ�����
        fade_Image = FindChild.OnFindChild<Image>(transform, "FadeOut_Image");//�H�X/�J�Ϥ�����
        tip_Text = FindChild.OnFindChild<Text>(transform, "Tip_Text");//���ܤ�r
        tip_Text.enabled = false;

        //���a���Ť�r
        playerLevel_Text = FindChild.OnFindChild<Text>(transform, "Level_Text");

        //���a���
        playerLifeBar_Image = FindChild.OnFindChild<Image>(transform, "LifaBar_Image");
        playerLifeBar_Image.type = Image.Type.Filled;
        playerLifeBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerLifeBar_Image.fillOrigin = 0;

        //���a�����r
        playerLifeBar_Text = FindChild.OnFindChild<Text>(transform, "LifeBar_Text");
        playerLifeBar_Text.alignment = TextAnchor.MiddleLeft;

        //���a�g���
        playerExperienceBar_Image = FindChild.OnFindChild<Image>(transform, "ExperienceBar_Image");
        playerExperienceBar_Image.type = Image.Type.Filled;
        playerExperienceBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerExperienceBar_Image.fillOrigin = 0;

        //���a�g�����r
        playerExperienceBar_Text = FindChild.OnFindChild<Text>(transform, "Experience_Text");
        playerExperienceBar_Text.alignment = TextAnchor.MiddleLeft;

        //���d���Ť�r
        gameLevel_Text = FindChild.OnFindChild<Text>(transform, "GameLevel_Text");
        OnSetGameLevel();//�]�w���d����

        //�D��Boss���s
        challengeBoss_Button = FindChild.OnFindChild<Button>(transform, "ChallengeBoss_Button");
        challengeBoss_Button.onClick.AddListener(OnChallengeBossButtonFunction);

        //BossUI��ܱ���
        bossUI_Transform = FindChild.OnFindChild<Transform>(transform, "BossUI");
        bossUI_Transform.gameObject.SetActive(false);

        //Boss���
        bossLifeBar = FindChild.OnFindChild<Image>(transform, "BossLifeBar_Image");
        bossLifeBar.type = Image.Type.Filled;
        bossLifeBar.fillMethod = Image.FillMethod.Horizontal;
        bossLifeBar.fillOrigin = 0;

        //Boss�����r
        bossLifeBar_Text = FindChild.OnFindChild<Text>(transform, "BossLifeBar_Text");
    }

    private void Update()
    {
        OnScreenFadeOut();//�e���H�X
        OnDeathTimeCountDown();//���`�ɶ��˼�
    }    

    /// <summary>
    /// UI�E��
    /// </summary>
    /// <param name="isActive">�O�_�E��</param>
    public void OnUIActive(bool isActive)
    {
        challengeBoss_Button.gameObject.SetActive(isActive);
        bossUI_Transform.gameObject.SetActive(!isActive);
    }

    /// <summary>
    /// �D��Boss���s�ƥ�
    /// </summary>
    void OnChallengeBossButtonFunction()
    {
        if (GameManagement.Instance.isPlayerDeath) return;
        GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//�M���ĤH
        GameManagement.Instance.OnCreateBoss();//����Boss
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();//��s�ƭ�
        OnUIActive(false);//UI�E��
        bossUI_Transform.gameObject.SetActive(true);
        bossLifeBar.fillAmount = 1;
        int boosHp = NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));
        bossLifeBar_Text.text = $"Hp: {boosHp} / {boosHp}";
    }

    /// <summary>
    /// �]�wBoss���
    /// </summary>
    /// <param name="hp">Hp</param>
    public void OnSetBossLifeBar(int hp)
    {        
        bossLifeBar.fillAmount = (float)hp /
            (float)(NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1)));
        bossLifeBar_Text.text = $"Hp: {hp} / " +
            $"{NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1))}";
    }

    /// <summary>
    /// �]�w���a���
    /// </summary>    
    /// <param name="hp">Hp</param>
    public void OnSetPlayerLifeBar(int hp)
    {
        playerLifeBar_Image.fillAmount = (float)hp / 
            (float)(NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1)));
        playerLifeBar_Text.text = $"Hp: {hp} / " +
            $"{NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1))}";
    }

    /// <summary>
    /// �]�w���a�g���
    /// </summary>    
    public void OnSetPlayerExperience()
    {
        float experienceRatio = (float)GameDataManagement.Instance.playerExperience /
                                (float)(((GameDataManagement.Instance.playerLevel - 1) * NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience) + NumericalValueManagement.NumericalValueManagement.upgradeExperience);
        playerExperienceBar_Image.fillAmount = experienceRatio;
        playerExperienceBar_Text.text = $"Exp: {GameDataManagement.Instance.playerExperience} / {NumericalValueManagement.NumericalValueManagement.upgradeExperience + (NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience * (GameDataManagement.Instance.playerLevel - 1))}";
    }

    /// <summary>
    /// �]�w���a����
    /// </summary>
    public void OnSetPlayerLevel()
    {
        playerLevel_Text.text = $"����: {++GameDataManagement.Instance.playerLevel}";
        GameDataManagement.Instance.playerExperience = 0;
    }

    /// <summary>
    /// �]�w���d����
    /// </summary>
    public void OnSetGameLevel()
    {
        gameLevel_Text.text = $"���d: {++GameDataManagement.Instance.gameLevel}";
    }

    /// <summary>
    /// �e���H�X
    /// </summary>
    void OnScreenFadeOut()
    {
        if (fadeAlpha > 0 && !GameManagement.Instance.isPlayerDeath)
        {
            fadeAlpha -= fadeSpeed * Time.deltaTime;
            fade_Image.color = new Color(0, 0, 0, fadeAlpha);
        }
    }

    /// <summary>
    /// ���`�ɶ��˼�
    /// </summary>
    void OnDeathTimeCountDown()
    {
        if (GameManagement.Instance.isPlayerDeath)
        {
            //�e���ܦ�
            if (fadeAlpha < deathAlpha)
            {
                fadeAlpha += Time.deltaTime;
                if (fadeAlpha >= deathAlpha) fadeAlpha = deathAlpha;
            }
            fade_Image.color = new Color(0, 0, 0, fadeAlpha);

            if (!tip_Text.enabled) tip_Text.enabled = true;

            //���ܤ�r
            deathTimeCountDown -= Time.deltaTime;
            if (deathTimeCountDown <= 0)
            {
                deathTimeCountDown = NumericalValueManagement.NumericalValueManagement.deathTime;//���`�ɶ�(�p�ɾ�)
                GameManagement.Instance.isPlayerDeath = false;
                tip_Text.enabled = false;

                GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//�M���ĤH
                GameManagement.Instance.OnRespawnPlayer();//���ͪ��a
                GameManagement.Instance.GetPlayerObject.OnUpdateValue();//��s�ƭ�
                OnUIActive(true);//UI�E��

                //�D��Boss
                if (GameManagement.Instance.isChallengeBoss)
                {
                    GameManagement.Instance.isChallengeBoss = false;
                    GameManagement.Instance.OnCleanEnemySoldier("BossObject", AssetManagement.Instance.bossObjects);//�M���ĤH
                }
            }

            tip_Text.text = $"{Convert.ToInt32(deathTimeCountDown).ToString()}";
        }
    }
}
