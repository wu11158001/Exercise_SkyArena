using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBehavior
{
    [Tooltip("CountDownTime")] float countDownTime;

    /// <summary>
    /// SkillCD
    /// </summary>
    /// <param name="skillNumber"></param>
    /// <param name="cd"></param>
    public void OnSkillCD(int skillNumber,float cd)
    {
        countDownTime -= Time.deltaTime;
        
        if(countDownTime <= 0)
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
                    OnSkillNumber_3();
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
    /// <param name="value"></param>
    void OnSkillNumber_1_Tornado(int value)
    {
       GameManagement.Instance.GetPlayerObject.OnSetDamageOverTimeAttack_Skill(effectName: "Tornado",
                                                                               damage: value);
    }

    /// <summary>
    /// SkillNumber_2_LightningBall
    /// </summary>
    /// <param name="value"></param>
    void OnSkillNumber_2_LightningBall(int value)
    {
        GameManagement.Instance.GetPlayerObject.OnSetBounceAttack(effectName: "LightningBall",
                                                                  damage: value);
    }

    /// <summary>
    /// SkillNumber_3
    /// </summary>
    void OnSkillNumber_3()
    {

    }
}
