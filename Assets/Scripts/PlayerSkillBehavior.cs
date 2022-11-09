using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBehavior
{
    [Tooltip("CountDownTime")] float countDownTime;

    /// <summary>
    /// SkillCD
    /// </summary>
    /// <param name="equipNumber"></param>
    /// <param name="skillNumber"></param>
    /// <param name="cd"></param>
    public void OnSkillCD(int equipNumber , int skillNumber,float cd)
    {
        countDownTime -= Time.deltaTime;
        GameUI.Instance.usingSkillsMask_Image[equipNumber].fillAmount = (float)countDownTime / (float)cd;

        if (countDownTime <= 0)
        {
            countDownTime = cd;
            int value = GameDataManagement.Instance.OnCalculateSkillValues(skillNumber);
            switch (skillNumber)
            {
                case 0:
                    OnSkillNumber_0_RecoverHp(value);
                    break;
                case 1:
                    OnSkillNumber_1_Tornado(value);
                    break;
                case 2:
                    OnSkillNumber_2_LightningBall(value);
                    break;
                case 3:
                    OnSkillNumber_3_Splash(value);
                    break;
                case 4:
                    OnSkillNumber_4_Slicer(value);
                    break;
            }
        }
    }

    /// <summary>
    /// SkillNumber_0_RecoverHp
    /// </summary>
    /// <param name="value"></param>
    void OnSkillNumber_0_RecoverHp(int value)
    {        
        GameManagement.Instance.GetPlayerObject.OnRecoverHp(value);        
    }

    /// <summary>
    /// OnSkillNumber_1_Tornado
    /// </summary>
    /// <param name="damage"></param>
    void OnSkillNumber_1_Tornado(int damage)
    {
       GameManagement.Instance.GetPlayerObject.OnSetDamageOverTimeAttack_Skill(effectName: "Tornado", damage: damage);
    }

    /// <summary>
    /// SkillNumber_2_LightningBall
    /// </summary>
    /// <param name="damage"></param>
    void OnSkillNumber_2_LightningBall(int damage)
    {
        GameManagement.Instance.GetPlayerObject.OnSetBounceAttack(effectName: "LightningBall", damage: damage);
    }

    /// <summary>
    /// SkillNumber_3_Splash
    /// </summary>
    /// <param name="damage"></param>
    void OnSkillNumber_3_Splash(int damage)
    {
        GameManagement.Instance.GetPlayerObject.OnSingleRandomAttack(effectName: "Splash", numberOfTime: 5 , damage: damage, soundEffectName: "Splash");
    }

    /// <summary>
    /// SkillNumber_4_Slicer
    /// </summary>
    /// <param name="damage"></param>
    void OnSkillNumber_4_Slicer(int damage)
    {
        GameManagement.Instance.GetPlayerObject.OnMoveForwardCollisionAttack(effectName: "Slicer", damage: damage);
    }
}
