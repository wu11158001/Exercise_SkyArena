using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 遊戲UI
/// </summary>
public class GameUI : MonoBehaviour
{
    static GameUI gameUI;
    public static GameUI Instance => gameUI;

    [Header("畫面淡出/入")]
    [SerializeField] [Tooltip("淡出/入圖片Alpha")] float fadeAlpha;
    [SerializeField] [Tooltip("淡出/入速度")] float fadeSpeed;
    [SerializeField] [Tooltip("死亡Alpha")] float deathAlpha;
    [Tooltip("淡出/入圖片物件")] Image fade_Image;

    [Header("提示文字")]
    [SerializeField] [Tooltip("死亡時間(計時器)")] float deathTimeCountDown;
    [Tooltip("提示文字")] Text tip_Text;

    [Header("玩家資訊")]
    [SerializeField] [Tooltip("玩家等級")] Text playerLevel_Text;
    [SerializeField] [Tooltip("玩家血條")] Image playerLifeBar_Image;
    [SerializeField] [Tooltip("玩家血條文字")] Text playerLifeBar_Text;
    [SerializeField] [Tooltip("玩家經驗條")] Image playerExperienceBar_Image;
    [SerializeField] [Tooltip("玩家經驗條文字")] Text playerExperienceBar_Text;
    [SerializeField] [Tooltip("關卡等級文字")] Text gameLevel_Text;

    [Header("挑戰Boss")]
    [SerializeField] [Tooltip("挑戰Boss按鈕")] Button challengeBoss_Button;
    [SerializeField] [Tooltip("BossUI顯示控制")] Transform bossUI_Transform;
    [SerializeField] [Tooltip("Boss血條")] Image bossLifeBar;
    [SerializeField] [Tooltip("Boss血條文字")] Text bossLifeBar_Text;


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
        OnGetComponent();//獲取Component

        //畫面淡出/入
        fadeAlpha = 1;//淡出/入圖片Alpha
        fade_Image.color = new Color(0, 0, 0, fadeAlpha);
        fadeSpeed = 0.3f;//淡出/入速度
        deathAlpha = 0.85f;//死亡Alpha

        //提示文字
        deathTimeCountDown = NumericalValueManagement.NumericalValueManagement.deathTime;//死亡時間(計時器)
    }

    /// <summary>
    /// 獲取Component
    /// </summary>
    void OnGetComponent()
    {
        //淡出/入圖片物件
        fade_Image = FindChild.OnFindChild<Image>(transform, "FadeOut_Image");//淡出/入圖片物件
        tip_Text = FindChild.OnFindChild<Text>(transform, "Tip_Text");//提示文字
        tip_Text.enabled = false;

        //玩家等級文字
        playerLevel_Text = FindChild.OnFindChild<Text>(transform, "Level_Text");

        //玩家血條
        playerLifeBar_Image = FindChild.OnFindChild<Image>(transform, "LifaBar_Image");
        playerLifeBar_Image.type = Image.Type.Filled;
        playerLifeBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerLifeBar_Image.fillOrigin = 0;

        //玩家血條文字
        playerLifeBar_Text = FindChild.OnFindChild<Text>(transform, "LifeBar_Text");
        playerLifeBar_Text.alignment = TextAnchor.MiddleLeft;

        //玩家經驗條
        playerExperienceBar_Image = FindChild.OnFindChild<Image>(transform, "ExperienceBar_Image");
        playerExperienceBar_Image.type = Image.Type.Filled;
        playerExperienceBar_Image.fillMethod = Image.FillMethod.Horizontal;
        playerExperienceBar_Image.fillOrigin = 0;

        //玩家經驗條文字
        playerExperienceBar_Text = FindChild.OnFindChild<Text>(transform, "Experience_Text");
        playerExperienceBar_Text.alignment = TextAnchor.MiddleLeft;

        //關卡等級文字
        gameLevel_Text = FindChild.OnFindChild<Text>(transform, "GameLevel_Text");
        OnSetGameLevel();//設定關卡等級

        //挑戰Boss按鈕
        challengeBoss_Button = FindChild.OnFindChild<Button>(transform, "ChallengeBoss_Button");
        challengeBoss_Button.onClick.AddListener(OnChallengeBossButtonFunction);

        //BossUI顯示控制
        bossUI_Transform = FindChild.OnFindChild<Transform>(transform, "BossUI");
        bossUI_Transform.gameObject.SetActive(false);

        //Boss血條
        bossLifeBar = FindChild.OnFindChild<Image>(transform, "BossLifeBar_Image");
        bossLifeBar.type = Image.Type.Filled;
        bossLifeBar.fillMethod = Image.FillMethod.Horizontal;
        bossLifeBar.fillOrigin = 0;

        //Boss血條文字
        bossLifeBar_Text = FindChild.OnFindChild<Text>(transform, "BossLifeBar_Text");
    }

    private void Update()
    {
        OnScreenFadeOut();//畫面淡出
        OnDeathTimeCountDown();//死亡時間倒數
    }    

    /// <summary>
    /// UI激活
    /// </summary>
    /// <param name="isActive">是否激活</param>
    public void OnUIActive(bool isActive)
    {
        challengeBoss_Button.gameObject.SetActive(isActive);
        bossUI_Transform.gameObject.SetActive(!isActive);
    }

    /// <summary>
    /// 挑戰Boss按鈕事件
    /// </summary>
    void OnChallengeBossButtonFunction()
    {
        if (GameManagement.Instance.isPlayerDeath) return;
        GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//清除敵人
        GameManagement.Instance.OnCreateBoss();//產生Boss
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();//更新數值
        OnUIActive(false);//UI激活
        bossUI_Transform.gameObject.SetActive(true);
        bossLifeBar.fillAmount = 1;
        int boosHp = NumericalValueManagement.NumericalValue_Boss.initial_Hp + (NumericalValueManagement.NumericalValue_Boss.raiseUpgradeHp * (GameDataManagement.Instance.gameLevel - 1));
        bossLifeBar_Text.text = $"Hp: {boosHp} / {boosHp}";
    }

    /// <summary>
    /// 設定Boss血條
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
    /// 設定玩家血條
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
    /// 設定玩家經驗條
    /// </summary>    
    public void OnSetPlayerExperience()
    {
        float experienceRatio = (float)GameDataManagement.Instance.playerExperience /
                                (float)(((GameDataManagement.Instance.playerLevel - 1) * NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience) + NumericalValueManagement.NumericalValueManagement.upgradeExperience);
        playerExperienceBar_Image.fillAmount = experienceRatio;
        playerExperienceBar_Text.text = $"Exp: {GameDataManagement.Instance.playerExperience} / {NumericalValueManagement.NumericalValueManagement.upgradeExperience + (NumericalValueManagement.NumericalValueManagement.raiseUpgradeExperience * (GameDataManagement.Instance.playerLevel - 1))}";
    }

    /// <summary>
    /// 設定玩家等級
    /// </summary>
    public void OnSetPlayerLevel()
    {
        playerLevel_Text.text = $"等級: {++GameDataManagement.Instance.playerLevel}";
        GameDataManagement.Instance.playerExperience = 0;
    }

    /// <summary>
    /// 設定關卡等級
    /// </summary>
    public void OnSetGameLevel()
    {
        gameLevel_Text.text = $"關卡: {++GameDataManagement.Instance.gameLevel}";
    }

    /// <summary>
    /// 畫面淡出
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
    /// 死亡時間倒數
    /// </summary>
    void OnDeathTimeCountDown()
    {
        if (GameManagement.Instance.isPlayerDeath)
        {
            //畫面變灰
            if (fadeAlpha < deathAlpha)
            {
                fadeAlpha += Time.deltaTime;
                if (fadeAlpha >= deathAlpha) fadeAlpha = deathAlpha;
            }
            fade_Image.color = new Color(0, 0, 0, fadeAlpha);

            if (!tip_Text.enabled) tip_Text.enabled = true;

            //提示文字
            deathTimeCountDown -= Time.deltaTime;
            if (deathTimeCountDown <= 0)
            {
                deathTimeCountDown = NumericalValueManagement.NumericalValueManagement.deathTime;//死亡時間(計時器)
                GameManagement.Instance.isPlayerDeath = false;
                tip_Text.enabled = false;

                GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);//清除敵人
                GameManagement.Instance.OnRespawnPlayer();//重生玩家
                GameManagement.Instance.GetPlayerObject.OnUpdateValue();//更新數值
                OnUIActive(true);//UI激活

                //挑戰Boss
                if (GameManagement.Instance.isChallengeBoss)
                {
                    GameManagement.Instance.isChallengeBoss = false;
                    GameManagement.Instance.OnCleanEnemySoldier("BossObject", AssetManagement.Instance.bossObjects);//清除敵人
                }
            }

            tip_Text.text = $"{Convert.ToInt32(deathTimeCountDown).ToString()}";
        }
    }
}
