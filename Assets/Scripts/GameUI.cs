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

    [Header("Judge")]
    [Tooltip("IsOpenInterface")] public bool isOpenInterface;

    [Header("FadeImage")]
    [Tooltip("FadeAlpha")] float fadeAlpha;
    [Tooltip("FadeSpeed")] float fadeSpeed;
    [Tooltip("DeathAlpha")] float deathAlpha;
    [Tooltip("Fade_Image")] Image fade_Image;

    [Header("TipText")]
    [Tooltip("DeathTimeCountDown")] float deathTimeCountDown;
    [Tooltip("Tip_Text")] Text tip_Text;

    [Header("PlayerUI")]
    [Tooltip("PlayerLevel_Text")] Text playerLevel_Text;
    [Tooltip("PlayerLifeBar_Image")] Image playerLifeBar_Image;
    [Tooltip("PlayerLifeBar_Text")] Text playerLifeBar_Text;
    [Tooltip("PlayerExperienceBar_Image")] Image playerExperienceBar_Image;
    [Tooltip("PlayerExperienceBar_Text")] Text playerExperienceBar_Text;
    [Tooltip("GameLevel_Text")] Text gameLevel_Text;
    [Tooltip("Gold_Text")] Text gold_Text;

    [Header("BossUI")]
    [Tooltip("ChallengeBoss_Button")] public Button challengeBoss_Button;
    [Tooltip("BossUI_Transform")] public Transform bossUI_Transform;
    [Tooltip("BossLifeBar")] Image bossLifeBar;
    [Tooltip("BossLifeBar_Text")] Text bossLifeBar_Text;

    [Header("AFK")]
    [Tooltip("AFKInterface")] Transform akfInterface;
    [Tooltip("AFK_Button")] Button afk_Button;
    [Tooltip("RewardTime_Text")] TMP_Text rewardTime_Text;
    [Tooltip("RewardConfirm_Button")] Button rewardConfirm_Button;
    [Tooltip("RewardExperience_Text")] TMP_Text rewardExperience_Text;
    [Tooltip("RewardGold_Text")] TMP_Text rewardGold_Text;

    [Header("UIAnimation")]
    [Tooltip("AFKButtonAniamtionCountDown")] float afkButtonAniamtionCountDown;
    [Tooltip("AFKButtonAnimationChangeTime")] float afkButtonAnimationChangeTime;
    [Tooltip("NowSprite_AFKButton")] int nowSprite_AFKButton;

    [Header("Skill")]
    [SerializeField] [Tooltip("SkillInterfaceBackground")] Image skillInterfaceBackground;
    [SerializeField] [Tooltip("UsingSkillBoxSprite")] Sprite usingSkillBoxSprite;
    [Tooltip("UsingSkills_Image")] Image[] usingSkills_Image;
    [Tooltip("UsingSkills_Text")] TMP_Text[] usingSkills_Text;
    [Tooltip("Skill_Button")] Button skill_Button;
    [Tooltip("SkillInterface")] Transform skillInterface;
    [Tooltip("SkillCancel_Button")] Button skillCancel_Button;
    [Tooltip("UsingSkillsMask_Image")] public Image[] usingSkillsMask_Image;

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
        OnSetUISize(skillInterfaceBackground);
        OnGetUI();

        //FadeImage
        fadeAlpha = 1;
        fade_Image.color = new Color(0, 0, 0, fadeAlpha);
        fadeSpeed = 0.3f;
        deathAlpha = 0.85f;

        //UIAnimation
        afkButtonAnimationChangeTime = 0.1f;

        deathTimeCountDown = NumericalValueManagement.NumericalValue_Game.deathTime;
    }

    /// <summary>
    /// SetUISize
    /// </summary>
    /// <param name="image"></param>
    void OnSetUISize(Image image)
    {
        if (image == null) return;
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    /// <summary>
    /// GetUI
    /// </summary>
    void OnGetUI()
    {
        //Fade_Image
        fade_Image = FindChild.OnFindChild<Image>(transform, "FadeOut_Image");
        tip_Text = FindChild.OnFindChild<Text>(transform, "Tip_Text");
        tip_Text.enabled = false;

        #region Player UI
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

        //Gold_Text
        gold_Text = FindChild.OnFindChild<Text>(transform, "Gold_Text");
        #endregion

        #region Boss UI
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
        #endregion

        #region AFK
        //AFKInterface
        akfInterface = FindChild.OnFindChild<Transform>(transform, "AFKInterface");
        akfInterface.gameObject.SetActive(false);

        //AFK RewardTime_Text
        rewardTime_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardTime_Text");

        //AFK Button
        afk_Button = FindChild.OnFindChild<Button>(transform, "AFK_Button");
        afk_Button.onClick.AddListener(delegate { OnSwitchInterface(akfInterface.gameObject, true); });        

        //AFK RewardConfirm_Button
        rewardConfirm_Button = FindChild.OnFindChild<Button>(transform, "RewardConfirm_Button");
        rewardConfirm_Button.onClick.AddListener(OnReceiveReward);

        //RewardExperience_Text
        rewardExperience_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardExperience_Text");

        //RewardGold_Text
        rewardGold_Text = FindChild.OnFindChild<TMP_Text>(transform, "RewardGold_Text");
        #endregion

        #region Skill
        //Skill_Button
        skill_Button = FindChild.OnFindChild<Button>(transform, "Skill_Button");
        skill_Button.onClick.AddListener(delegate { OnSwitchInterface(skillInterface.gameObject, true); });

        //SkillInterface
        skillInterface = FindChild.OnFindChild<Transform>(transform, "SkillInterface");
        skillInterface.gameObject.SetActive(false);

        //SkillCancel_Button
        skillCancel_Button = FindChild.OnFindChild<Button>(transform, "SkillCancel_Button");
        skillCancel_Button.onClick.AddListener(delegate { OnSwitchInterface(skillInterface.gameObject, false); });

        //UsingSkills(Image Text Mask)
        usingSkills_Image = new Image[GameDataManagement.Instance.playEquipSkillNumber.Length];
        usingSkillsMask_Image = new Image[GameDataManagement.Instance.playEquipSkillNumber.Length];
        usingSkills_Text = new TMP_Text[GameDataManagement.Instance.playEquipSkillNumber.Length];        
        for (int i = 0; i < GameDataManagement.Instance.playEquipSkillNumber.Length; i++)
        {
            usingSkills_Image[i] = FindChild.OnFindChild<Image>(transform, $"UsingSkill{i + 1}_Image");
            usingSkillsMask_Image[i] = FindChild.OnFindChild<Image>(transform, $"UsingSkillMask{i + 1}_Image");
            usingSkills_Text[i] = FindChild.OnFindChild<TMP_Text>(transform, $"UsingSkill{i + 1}_Text");            
        }
        OnSetPlayerUsingSkill(usingSkills_Image, usingSkills_Text);
        #endregion
    }

    private void Update()
    {
        OnScreenFadeOut();
        OnDeathTimeCountDown();
        OnSetRewardTime();

        OnUIAnimation(ref afkButtonAniamtionCountDown, afkButtonAnimationChangeTime, afk_Button.image, AssetManagement.Instance.afkButtonSprite, ref nowSprite_AFKButton);
    }   

    /// <summary>
    /// SetPlayerUsingSkill
    /// </summary>
    /// <param name="images"></param>
    /// <param name="texts"></param>
    public void OnSetPlayerUsingSkill(Image[] images, TMP_Text[] texts)
    {
        for (int i = 0; i < GameDataManagement.Instance.playEquipSkillNumber.Length; i++)
        {
            if (GameDataManagement.Instance.playEquipSkillNumber[i] >= 0)
            {
                images[i].sprite = AssetManagement.Instance.skillIconSprite[GameDataManagement.Instance.playEquipSkillNumber[i]];
                texts[i].text = GameDataManagement.Instance.allSkillGrade[GameDataManagement.Instance.playEquipSkillNumber[i]].ToString();
            }
            else
            {
                images[i].sprite = usingSkillBoxSprite;
                texts[i].text = "";
            }
        }
    }

    /// <summary>
    /// UIAnimation
    /// </summary>
    /// <param name="countDownTime"></param>
    /// <param name="changeTime"></param>
    /// <param name="image"></param>
    /// <param name="sprites"></param>
    void OnUIAnimation(ref float countDownTime, float changeTime, Image image, Sprite[] assetBundleObject, ref int nowSprite)
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime <= 0)
        {
            countDownTime = changeTime;
            nowSprite = nowSprite > assetBundleObject.Length - 2 ? 0 : ++nowSprite;
            image.sprite = assetBundleObject[nowSprite];
        }
    }

    #region AFK
    /// <summary>
    /// ReceiveReward
    /// </summary>
    void OnReceiveReward()
    {
        akfInterface.gameObject.SetActive(false);
        GameDataManagement.Instance.afkRewardStartTime = DateTime.Now;
        GameDataManagement.Instance.playerExperience += GameDataManagement.Instance.afkExperienceReward;
        GameDataManagement.Instance.playerGold += GameDataManagement.Instance.afkGoldReward;
        GameDataManagement.Instance.OnSaveJsonData();
 
        OnSetPlayerExperience();
        OnSetPlayerGold();

        isOpenInterface = false;
    }

    /// <summary>
    /// SetRewardTime
    /// </summary>
    void OnSetRewardTime()
    {
        string hour = "", minute = "", second = "";

        int total = (int)(GameDataManagement.Instance.OnAFKRewardTime().TotalSeconds);
        if (total > 86400) total = 86400;

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

        //Afk Time
        rewardTime_Text.text = $"{hour} : {minute} : {second}";

        //Experience        
        GameDataManagement.Instance.afkExperienceReward = (total / NumericalValueManagement.NumericalValue_Game.afkRewardTiming) * NumericalValueManagement.NumericalValue_Game.akfExperienceReward;
        rewardExperience_Text.text = $"EP: {GameDataManagement.Instance.afkExperienceReward}";

        //Gold
        GameDataManagement.Instance.afkGoldReward = (total / NumericalValueManagement.NumericalValue_Game.afkRewardTiming) * NumericalValueManagement.NumericalValue_Game.akfGoldReward;
        rewardGold_Text.text = $"Gold: {GameDataManagement.Instance.afkGoldReward}";
    }
    #endregion
   
    /// <summary>
    /// ChallengeBossButtonFunction
    /// </summary>
    void OnChallengeBossButtonFunction()
    {
        if (GameManagement.Instance.isPlayerDeath) return;
        GameManagement.Instance.OnCleanEnemySoldier("EnemySoldierObject", AssetManagement.Instance.enemySoldierObjects);
        GameManagement.Instance.OnCreateBoss();
        GameManagement.Instance.GetPlayerObject.OnUpdateValue();
        challengeBoss_Button.gameObject.SetActive(false);
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
    /// SetPlayerGold
    /// </summary>
    public void OnSetPlayerGold()
    {
        gold_Text.text = $"Gold: {GameDataManagement.Instance.playerGold}";
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

                challengeBoss_Button.gameObject.SetActive(true);

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

    /// <summary>
    /// SwitchInterface
    /// </summary>
    void OnSwitchInterface(GameObject item, bool isActive)
    {
        if (isActive == true && isOpenInterface == true) return;
        
        isOpenInterface = isActive;
        item.SetActive(isActive);

        OnSetPlayerUsingSkill(usingSkills_Image, usingSkills_Text);
    }
}
