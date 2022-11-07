using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInterface : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] [Tooltip("SelectSkillNumber")] int selectSkillNumber;
    [Tooltip("SkillsString")] readonly string[] skillsString = new string[] { "Skill_1", "Skill_2", "Skill_3", "Skill_4" };    

    [Header("UI")]    
    [Tooltip("SkillButtons")] Button[] skillButtons;
    [Tooltip("IllustrateSkill_Text")] TMP_Text illustrateSkill_Text;
    [Tooltip("SelectSkill_Image")] Image selectSkill_Image;
    [Tooltip("UpGradeSkill_Button")] Button upGradeSkill_Button;
    [Tooltip("SkillGold_Text")] TMP_Text skillGold_Text;
    [Tooltip("SkillTip_Text")] TMP_Text skillTip_Text;
    [Tooltip("EquipSkill_Button")] Button equipSkill_Button;
    [Tooltip("PlayerEquipSkills_Button")] Button[] playerEquipSkills_Button;
    [Tooltip("PlayerEquipSkills_Image")] Image[] PlayerEquipSkills_Image;
    [Tooltip("PlayerEquipSkills_Text")] TMP_Text[] PlayerEquipSkills_Text;
    [Tooltip("UnEquipSkill_Button")] Button unEquipSkill_Button;    

    private void Start()
    {
        OnGetUI();
    }

    /// <summary>
    /// GetUI
    /// </summary>
    void OnGetUI()
    {
        //SkillButtons
        skillButtons = new Button[GameDataManagement.Instance.allSkillGrade.Length];
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i] = FindChild.OnFindChild<Button>(transform, $"Skill{i + 1}_Button");
            skillButtons[i].image.sprite = AssetManagement.Instance.skillIconSprite[i];
            skillButtons[i].GetComponentInChildren<TMP_Text>().text = GameDataManagement.Instance.allSkillGrade[i].ToString();
            OnSetSkillButtonFunction(skillButtons[i], i);
        }

        //IllustrateSkill_Text
        illustrateSkill_Text = FindChild.OnFindChild<TMP_Text>(transform, "IllustrateSkill_Text");
        illustrateSkill_Text.text = "";

        //SelectSkill_Image
        selectSkill_Image = FindChild.OnFindChild<Image>(transform, "SelectSkill_Image");

        //UpGradeSkill_Button
        upGradeSkill_Button = FindChild.OnFindChild<Button>(transform, "UpGradeSkill_Button");
        upGradeSkill_Button.gameObject.SetActive(false);
        upGradeSkill_Button.onClick.AddListener(OnUpGradeSkill);

        //SkillGold_Text
        skillGold_Text = FindChild.OnFindChild<TMP_Text>(transform, "SkillGold_Text");
        skillGold_Text.text = $"Gold: {GameDataManagement.Instance.playerGold.ToString()}";

        //SkillTip_Text
        skillTip_Text = FindChild.OnFindChild<TMP_Text>(transform, "SkillTip_Text");
        skillTip_Text.enabled = false;

        //EquipSkill_Button
        equipSkill_Button = FindChild.OnFindChild<Button>(transform, "EquipSkill_Button");
        equipSkill_Button.gameObject.SetActive(false);
        equipSkill_Button.onClick.AddListener(OnEquipSkill);

        //PlayerEquipSkills_Button
        playerEquipSkills_Button = new Button[GameDataManagement.Instance.playEquipSkillNumber.Length];
        PlayerEquipSkills_Image = new Image[GameDataManagement.Instance.playEquipSkillNumber.Length];
        PlayerEquipSkills_Text = new TMP_Text[GameDataManagement.Instance.playEquipSkillNumber.Length];
        for (int i = 0; i < playerEquipSkills_Button.Length; i++)
        {
            playerEquipSkills_Button[i] = FindChild.OnFindChild<Button>(transform, $"PlayerEquipSkill{i + 1}_Button");
            PlayerEquipSkills_Image[i] = playerEquipSkills_Button[i].image;
            PlayerEquipSkills_Text[i] = playerEquipSkills_Button[i].GetComponentInChildren<TMP_Text>();
            OnSetEquipSkillButtonFunction(playerEquipSkills_Button[i], i);
        }
        GameUI.Instance.OnSetPlayerUsingSkill(PlayerEquipSkills_Image, PlayerEquipSkills_Text);

        //UnEquipSkill_Button
        unEquipSkill_Button = FindChild.OnFindChild<Button>(transform, "UnEquipSkill_Button");
        unEquipSkill_Button.gameObject.SetActive(false);
        unEquipSkill_Button.onClick.AddListener(OnUnEquip);        
    }

    private void Update()
    {
        OnKeepUpdateUI();        
    }

    /// <summary>
    /// KeepUpdateUI
    /// </summary>
    void OnKeepUpdateUI()
    {
        //Player Gold
        skillGold_Text.text = $"Gold: {GameDataManagement.Instance.playerGold.ToString()}";
        
        for (int i = 0; i < GameDataManagement.Instance.playEquipSkillNumber.Length; i++)
        {
            //UnEquipSkill_Button
            if (GameDataManagement.Instance.playEquipSkillNumber[i] == selectSkillNumber)
            {
                unEquipSkill_Button.gameObject.SetActive(true);
            }
            else unEquipSkill_Button.gameObject.SetActive(false);

            //EquipSkill_Button
            if (GameDataManagement.Instance.allSkillGrade[selectSkillNumber] <= 0 || GameDataManagement.Instance.playEquipSkillNumber[i] == selectSkillNumber)
            {               
                equipSkill_Button.gameObject.SetActive(false);
                break;
            }
            else equipSkill_Button.gameObject.SetActive(true);            
        }       
    }

    /// <summary>
    /// UnEquip
    /// </summary>
    void OnUnEquip()
    {
        for (int i = 0; i < GameDataManagement.Instance.playEquipSkillNumber.Length; i++)
        {
            if(GameDataManagement.Instance.playEquipSkillNumber[i] == selectSkillNumber)
            {
                GameDataManagement.Instance.playEquipSkillNumber[i] = -1;                
                break;
            }
        }

        GameUI.Instance.OnSetPlayerUsingSkill(PlayerEquipSkills_Image, PlayerEquipSkills_Text);
    }

    /// <summary>
    /// ClickEquipPlayerEquipSkills
    /// </summary>
    /// <param name="number"></param>
    void OnClickEquipPlayerEquipSkills(int number)
    {
        OnClickSkill(number);        
    }

    /// <summary>
    /// SetEquipSkillButtonFunction
    /// </summary>
    /// <param name="button"></param>
    void OnSetEquipSkillButtonFunction(Button button, int i)
    {
        button.onClick.AddListener(delegate { OnClickEquipPlayerEquipSkills(GameDataManagement.Instance.playEquipSkillNumber[i]); });
    }

    /// <summary>
    /// EquipSkill
    /// </summary>
    void OnEquipSkill()
    {
        for (int i = 0; i < GameDataManagement.Instance.playEquipSkillNumber.Length; i++)
        {
            if(GameDataManagement.Instance.playEquipSkillNumber[i] < 0 && GameDataManagement.Instance.playEquipSkillNumber[i] != selectSkillNumber)
            {
                GameDataManagement.Instance.playEquipSkillNumber[i] = selectSkillNumber;
                playerEquipSkills_Button[i].image.sprite = AssetManagement.Instance.skillIconSprite[selectSkillNumber];
                playerEquipSkills_Button[i].GetComponentInChildren<TMP_Text>().text = GameDataManagement.Instance.allSkillGrade[selectSkillNumber].ToString();                                
                break;
            }
        }        
    }

    /// <summary>
    /// UpGradeSkill
    /// </summary>
    void OnUpGradeSkill()
    {
        //Lack Of Gold 
        if (GameDataManagement.Instance.playerGold - 
            (NumericalValueManagement.NumericalValue_Game.skillinItialCost + (GameDataManagement.Instance.allSkillGrade[selectSkillNumber] *                 NumericalValueManagement.NumericalValue_Game.skillRaiseCost)) < 0)
        {
            skillTip_Text.enabled = true;
            skillTip_Text.text = "Not Enough Gold!";
            Invoke(nameof(OnCloseTipText), 1);
            return;
        }

        //Reduce Of Gold
        GameDataManagement.Instance.playerGold -=
            NumericalValueManagement.NumericalValue_Game.skillinItialCost +
            (GameDataManagement.Instance.allSkillGrade[selectSkillNumber] * NumericalValueManagement.NumericalValue_Game.skillRaiseCost);      

        //UpGrade Skill
        GameDataManagement.Instance.allSkillGrade[selectSkillNumber]++;
        skillButtons[selectSkillNumber].GetComponentInChildren<TMP_Text>().text = GameDataManagement.Instance.allSkillGrade[selectSkillNumber].ToString();
        OnSkillButtonText();
        GameUI.Instance.OnSetPlayerUsingSkill(PlayerEquipSkills_Image, PlayerEquipSkills_Text);

        OnUpdateSkillImformation();
    }

    /// <summary>
    /// SetSkillButtonFunction
    /// </summary>
    /// <param name="button"></param>
    /// <param name="i"></param>
    void OnSetSkillButtonFunction(Button button, int i)
    {
        button.onClick.AddListener(delegate { OnClickSkill(i); });
    }

    /// <summary>
    /// OnClickSkill
    /// </summary>
    /// <param name="skillNumber"></param>
    void OnClickSkill(int skillNumber)
    {
        if (skillNumber < 0) return;

        selectSkillNumber = skillNumber;

        //UpGradeSkill && EquipSkill Button
        upGradeSkill_Button.gameObject.SetActive(true);

        selectSkill_Image.sprite = AssetManagement.Instance.skillIconSprite[skillNumber];

        OnUpdateSkillImformation();
        OnSkillButtonText();
    }

    /// <summary>
    /// UpdateSkillImformation
    /// </summary>    
    void OnUpdateSkillImformation()
    {
        illustrateSkill_Text.text = $"CD: {NumericalValueManagement.NumbericalValue_PlayerSkill.playerSkillsCD[selectSkillNumber]}\n" +
                                    $"{NumericalValueManagement.NumbericalValue_PlayerSkill.skillInformation[selectSkillNumber]}" +
                                    $"{GameDataManagement.Instance.OnCalculateSkillValues(selectSkillNumber)}";
    }

    /// <summary>
    /// SkillButtonText
    /// </summary>
    void OnSkillButtonText()
    {
        //Select Skill Grade
        selectSkill_Image.GetComponentInChildren<TMP_Text>().text = GameDataManagement.Instance.allSkillGrade[selectSkillNumber].ToString();

        //UpGrade Skill Cost
        string text = GameDataManagement.Instance.allSkillGrade[selectSkillNumber] == 0 ? "Learning" : "UpGrade";
        upGradeSkill_Button.GetComponentInChildren<TMP_Text>().text =
            $"{NumericalValueManagement.NumericalValue_Game.skillinItialCost + (GameDataManagement.Instance.allSkillGrade[selectSkillNumber] * NumericalValueManagement.NumericalValue_Game.skillRaiseCost)}\n{text}";
    }

    /// <summary>
    /// CloseTipText
    /// </summary>
    void OnCloseTipText()
    {
        skillTip_Text.enabled = false;        
    }
}