using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour
{
    static GameUI gameUI;
    public static GameUI Instance => gameUI;

    [Header("FadeImage")]
    [SerializeField] [Tooltip("FadeAlpha")] float fadeAlpha;
    [SerializeField] [Tooltip("FadeSpeed")] float fadeSpeed;
    [SerializeField] [Tooltip("DeathAlpha")] float deathAlpha;
    [Tooltip("Fade_Image")] Image fade_Image;

    [Header("TipText")]
    [SerializeField] [Tooltip("DeathTimeCountDown")] float deathTimeCountDown;
    [Tooltip("Tip_Text")] Text tip_Text;

    [Header("PlayerUI")]
    [SerializeField] [Tooltip("PlayerLevel_Text")] Text playerLevel_Text;
    [SerializeField] [Tooltip("PlayerLifeBar_Image")] Image playerLifeBar_Image;
    [SerializeField] [Tooltip("PlayerLifeBar_Text")] Text playerLifeBar_Text;
    [SerializeField] [Tooltip("PlayerExperienceBar_Image")] Image playerExperienceBar_Image;
    [SerializeField] [Tooltip("PlayerExperienceBar_Text")] Text playerExperienceBar_Text;
    [SerializeField] [Tooltip("GameLevel_Text")] Text gameLevel_Text;

    [Header("BossUI")]
    [SerializeField] [Tooltip("ChallengeBoss_Button")] Button challengeBoss_Button;
    [SerializeField] [Tooltip("BossUI_Transform")] Transform bossUI_Transform;
    [SerializeField] [Tooltip("BossLifeBar")] Image bossLifeBar;
    [SerializeField] [Tooltip("BossLifeBar_Text")] Text bossLifeBar_Text;


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
        OnGetComponent();

        fadeAlpha = 1;
        fade_Image.color = new Color(0, 0, 0, fadeAlpha);
        fadeSpeed = 0.3f;
        deathAlpha = 0.85f;

        deathTimeCountDown = NumericalValueManagement.NumericalValue_Game.deathTime;
    }

    /// <summary>
    /// GetComponent
    /// </summary>
    void OnGetComponent()
    {
        //Fade_Image
        fade_Image = FindChild.OnFindChild<Image>(transform, "FadeOut_Image");
        tip_Text = FindChild.OnFindChild<Text>(transform, "Tip_Text");
        tip_Text.enabled = false;

        //PlayerLevel_Text
        playerLevel_Text = FindChild.OnFindChild<Text>(transform, "Level_Text");

        //PlayerLifeBar_Image
        playerLifeBar_Image = FindChild.OnFindChild<Image>(transform, "LifaBar_Image");
        playerLifeBar_Image.type = Image.Type.Filled;
        playerLifeBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerLifeBar_Image.fillOrigin = 0;

        //PlayerLifeBar_Text
        playerLifeBar_Text = FindChild.OnFindChild<Text>(transform, "LifeBar_Text");
        playerLifeBar_Text.alignment = TextAnchor.MiddleLeft;

        //PlayerExperienceBar_Image
        playerExperienceBar_Image = FindChild.OnFindChild<Image>(transform, "ExperienceBar_Image");
        playerExperienceBar_Image.type = Image.Type.Filled;
        playerExperienceBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerExperienceBar_Image.fillOrigin = 0;

        //PlayerExperienceBar_Text
        playerExperienceBar_Text = FindChild.OnFindChild<Text>(transform, "Experience_Text");
        playerExperienceBar_Text.alignment = TextAnchor.MiddleLeft;

        //GameLevel_Text
        gameLevel_Text = FindChild.OnFindChild<Text>(transform, "GameLevel_Text");
        OnSetGameLevel();//設定關卡等級

        //ChallengeBoss_Button
        challengeBoss_Button = FindChild.OnFindChild<Button>(transform, "ChallengeBoss_Button");
        challengeBoss_Button.onClick.AddListener(OnChallengeBossButtonFunction);

        //BossUI ActiveControl
        bossUI_Transform = FindChild.OnFindChild<Transform>(transform, "BossUI");
        bossUI_Transform.gameObject.SetActive(false);

        //BossLifeBar_Image
        bossLifeBar = FindChild.OnFindChild<Image>(transform, "BossLifeBar_Image");
        bossLifeBar.type = Image.Type.Filled;
        bossLifeBar.fillMethod = Image.FillMethod.Horizontal;
        bossLifeBar.fillOrigin = 0;

        //BossLifeBar_Text
        bossLifeBar_Text = FindChild.OnFindChild<Text>(transform, "BossLifeBar_Text");
    }

    private void Update()
    {
        OnScreenFadeOut();
        OnDeathTimeCountDown();
    }

    /// <summary>
    ///nUIActive
    /// </summary>
    /// <param name="isActive"></param>
    public void OnUIActive(bool isActive)
    {
        challengeBoss_Button.gameObject.SetActive(isActive);
        bossUI_Transform.gameObject.SetActive(!isActive);
    }

    /// <summary>
    /// ChallengeBossButtonFunction
    /// </summary>
    void OnChallengeBossButtonFunction()
    {
        if (GameManagement.Instance.isPlayerDeath) return;
        GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);
        GameManagement.Instance.OnCreateBoss();
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();
        OnUIActive(false);
        bossUI_Transform.gameObject.SetActive(true);
        bossLifeBar.fillAmount = 1;
        int boosHp = NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));
        bossLifeBar_Text.text = $"Hp: {boosHp} / {boosHp}";
    }

    /// <summary>
    /// SetBossLifeBar
    /// </summary>
    /// <param name="hp"></param>
    public void OnSetBossLifeBar(int hp)
    {        
        bossLifeBar.fillAmount = (float)hp /
            (float)(NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1)));
        bossLifeBar_Text.text = $"Hp: {hp} / " +
            $"{NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1))}";
    }

    /// <summary>
    /// OnSetPlayerLifeBar
    /// </summary>
    /// <param name="hp"></param>
    public void OnSetPlayerLifeBar(int hp)
    {
        playerLifeBar_Image.fillAmount = (float)hp / 
            (float)(NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1)));
        playerLifeBar_Text.text = $"Hp: {hp} / " +
            $"{NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerLevel - 1))}";
    }

    /// <summary>
    /// SetPlayerExperience
    /// </summary>
    public void OnSetPlayerExperience()
    {
        float experienceRatio = (float)GameDataManagement.Instance.playerExperience /
                                (float)(((GameDataManagement.Instance.playerLevel - 1) * NumericalValueManagement.NumericalValue_Game.raiseUpgradeExperience) + NumericalValueManagement.NumericalValue_Game.upgradeExperience);
        playerExperienceBar_Image.fillAmount = experienceRatio;
        playerExperienceBar_Text.text = $"Exp: {GameDataManagement.Instance.playerExperience} / " +
            $"{NumericalValueManagement.NumericalValue_Game.upgradeExperience + (NumericalValueManagement.NumericalValue_Game.raiseUpgradeExperience * (GameDataManagement.Instance.playerLevel - 1))}";
    }

    /// <summary>
    /// SetPlayerFrade
    /// </summary>
    public void OnSetPlayerGrade()
    {
        playerLevel_Text.text = $"Grade: {++GameDataManagement.Instance.playerLevel}";
        GameDataManagement.Instance.playerExperience = 0;
    }

    /// <summary>
    /// SetGameLevel
    /// </summary>
    public void OnSetGameLevel()
    {
        gameLevel_Text.text = $"LV: {++GameDataManagement.Instance.gameLevel}";
    }

    /// <summary>
    /// ScreenFadeOut
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
    /// DeathTimeCountDown
    /// </summary>
    void OnDeathTimeCountDown()
    {
        if (GameManagement.Instance.isPlayerDeath)
        {
            //FadeIn
            if (fadeAlpha < deathAlpha)
            {
                fadeAlpha += Time.deltaTime;
                if (fadeAlpha >= deathAlpha) fadeAlpha = deathAlpha;
            }
            fade_Image.color = new Color(0, 0, 0, fadeAlpha);

            if (!tip_Text.enabled) tip_Text.enabled = true;

            //Death Over
            deathTimeCountDown -= Time.deltaTime;
            if (deathTimeCountDown <= 0)
            {
                deathTimeCountDown = NumericalValueManagement.NumericalValue_Game.deathTime;
                GameManagement.Instance.isPlayerDeath = false;
                tip_Text.enabled = false;

                GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);
                GameManagement.Instance.OnRespawnPlayer();
                GameManagement.Instance.GetPlayerObject.OnUpdateValue();
                OnUIActive(true);

                //Challenge Boss
                if (GameManagement.Instance.isChallengeBoss)
                {
                    GameManagement.Instance.isChallengeBoss = false;
                    GameManagement.Instance.OnCleanEnemySoldier("BossObject", AssetManagement.Instance.bossObjects);
                }
            }

            tip_Text.text = $"{Convert.ToInt32(deathTimeCountDown).ToString()}";
        }
    }
}
