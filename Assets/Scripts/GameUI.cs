using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameUI : MonoBehaviour
{
    static GameUI gameUI;
    public static GameUI Instance => gameUI;

    [Header("FadeImage")]
    [Tooltip("FadeAlpha")] float fadeAlpha;
    [Tooltip("FadeSpeed")] float fadeSpeed;
    [Tooltip("DeathAlpha")] float deathAlpha;
    [Tooltip("Fade_Image")] Image fade_Image;

    [Header("TipText")]
    [SerializeField] [Tooltip("DeathTimeCountDown")] float deathTimeCountDown;
    [Tooltip("Tip_Text")] Text tip_Text;

    [Header("PlayerUI")]
    [Tooltip("PlayerLevel_Text")] Text playerLevel_Text;
    [Tooltip("PlayerLifeBar_Image")] Image playerLifeBar_Image;
    [Tooltip("PlayerLifeBar_Text")] Text playerLifeBar_Text;
    [Tooltip("PlayerExperienceBar_Image")] Image playerExperienceBar_Image;
    [Tooltip("PlayerExperienceBar_Text")] Text playerExperienceBar_Text;
    [Tooltip("GameLevel_Text")] Text gameLevel_Text;

    [Header("BossUI")]
    [Tooltip("ChallengeBoss_Button")] Button challengeBoss_Button;
    [Tooltip("BossUI_Transform")] Transform bossUI_Transform;
    [Tooltip("BossLifeBar")] Image bossLifeBar;
    [Tooltip("BossLifeBar_Text")] Text bossLifeBar_Text;

    [Header("AFKReward")]
    [Tooltip("AFKRewardBackground_Image")] Transform afkRewardBackground_Image;
    [Tooltip("AFKReward_Button")] Button afkReward_Button;
    [Tooltip("RewardTime_Text")] TMP_Text rewardTime_Text;
    [Tooltip("RewardConfirm_Button")] Button rewardConfirm_Button;
    [Tooltip("RewardExperience_Text")] TMP_Text rewardExperience_Text;
    [Tooltip("RewardGold_Text")] TMP_Text rewardGold_Text;

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

        //AFK RewardBackground_Image
        afkRewardBackground_Image = FindChild.OnFindChild<Transform>(transform, "AFKRewardBackground_Image");
        afkRewardBackground_Image.gameObject.SetActive(false);

        //AFK RewardTime_Text
        rewardTime_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardTime_Text");

        //AFK Reward_Button
        afkReward_Button = FindChild.OnFindChild<Button>(transform, "AFKReward_Button");
        afkReward_Button.onClick.AddListener(OnReceiveAFKReward);

        //AFK RewardConfirm_Button
        rewardConfirm_Button = FindChild.OnFindChild<Button>(transform, "RewardConfirm_Button");
        rewardConfirm_Button.onClick.AddListener(OnReceiveReward);

        //RewardExperience_Text
        rewardExperience_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardExperience_Text");

        //RewardGold_Text
        rewardGold_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardGold_Text");
        
    }

    private void Update()
    {
        OnScreenFadeOut();
        OnDeathTimeCountDown();
        OnSetRewardTime();
    }

    #region AFK
    /// <summary>
    /// ReceiveReward
    /// </summary>
    void OnReceiveReward()
    {
        afkRewardBackground_Image.gameObject.SetActive(false);
        GameDataManagement.Instance.afkRewardStartTime = DateTime.Now;
        GameDataManagement.Instance.OnSaveJsonData();
    }

    /// <summary>
    /// SetRewardTime
    /// </summary>
    void OnSetRewardTime()
    {
        string hour = "", minute = "", second = "";

        int total = (int)(GameDataManagement.Instance.OnAFKRewardTime().TotalSeconds);

        //Second
        if (total % 60 == 0) second = "00";
        else second = total - (60 * (int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes) < 10 ?
                $"0{ (total - (60 * (int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes)).ToString()}" :
                $"{(total - (60 * (int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes)).ToString()}";

        //Minute
        if ((int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes == 0) minute = "00";
        else minute = (int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes < 10 ?
                $"0{(int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes}"
                : $"{(int)GameDataManagement.Instance.OnAFKRewardTime().TotalMinutes}";

        //Hour
        if ((int)GameDataManagement.Instance.OnAFKRewardTime().TotalHours == 0) hour = "00";
        else hour = (int)GameDataManagement.Instance.OnAFKRewardTime().TotalHours < 10 ?
                $"0{(int)GameDataManagement.Instance.OnAFKRewardTime().TotalHours}"
                : $"{(int)GameDataManagement.Instance.OnAFKRewardTime().TotalHours}";

        //Experience        
        GameDataManagement.Instance.afkExperienceReward = (total / NumericalValueManagement.NumericalValue_Game.afkRewardTiming) * NumericalValueManagement.NumericalValue_Game.akfExperienceReward;
        rewardExperience_Text.text = $"{GameDataManagement.Instance.afkExperienceReward}";

        //RewardGold_Text
        GameDataManagement.Instance.afkGoldReward = (total / NumericalValueManagement.NumericalValue_Game.afkRewardTiming) * NumericalValueManagement.NumericalValue_Game.akfGoldReward;
        rewardGold_Text.text = $"{GameDataManagement.Instance.afkGoldReward}";

        rewardTime_Text.text = $"{hour} : {minute} : {second}";
    }

    /// <summary>
    /// ReceiveAFKReward
    /// </summary>
    void OnReceiveAFKReward()
    {
        afkRewardBackground_Image.gameObject.SetActive(true);
    }
    #endregion

    /// <summary>
    ///UIActive
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
            (float)(NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerGrade - 1)));
        playerLifeBar_Text.text = $"Hp: {hp} / " +
            $"{NumericalValueManagement.NumericalValue_Player.initial_Hp + (NumericalValueManagement.NumericalValue_Player.raiseUpgradeHp * (GameDataManagement.Instance.playerGrade - 1))}";
    }

    /// <summary>
    /// SetPlayerExperience
    /// </summary>
    public void OnSetPlayerExperience()
    {
        float experienceRatio = (float)GameDataManagement.Instance.playerExperience /
                                (float)(((GameDataManagement.Instance.playerGrade - 1) * NumericalValueManagement.NumericalValue_Game.raiseUpgradeExperience) + NumericalValueManagement.NumericalValue_Game.upgradeExperience);
        playerExperienceBar_Image.fillAmount = experienceRatio;
        playerExperienceBar_Text.text = $"Exp: {GameDataManagement.Instance.playerExperience} / " +
            $"{NumericalValueManagement.NumericalValue_Game.upgradeExperience + (NumericalValueManagement.NumericalValue_Game.raiseUpgradeExperience * (GameDataManagement.Instance.playerGrade - 1))}";
    }

    /// <summary>
    /// SetPlayerFrade
    /// </summary>
    public void OnSetPlayerGrade()
    {
        playerLevel_Text.text = $"Grade: {GameDataManagement.Instance.playerGrade}";
    }

    /// <summary>
    /// SetGameLevel
    /// </summary>
    public void OnSetGameLevel()
    {
        gameLevel_Text.text = $"LV: {GameDataManagement.Instance.gameLevel}";
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
                    GameManagement.Instance.OnCleanBoss("BossObject", AssetManagement.Instance.boss_List);
                }
            }

            tip_Text.text = $"{Convert.ToInt32(deathTimeCountDown).ToString()}";
        }
    }
}
