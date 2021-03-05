using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 攻击伤害类：
 * 该类包含一系列伤害数据以及攻击效果等
 * 
 */
public class AttackHurt : EventArgs {

    public int Hurt { get; set; }
    
    /// <summary>
    /// 实例化攻击伤害类
    /// </summary>
    /// <param name="p_Hurt"></param>
    public AttackHurt(int p_Hurt)
    {
        this.Hurt = p_Hurt;
    }

    /// <summary>
    /// 伤害计算方法
    /// </summary>
    /// <param name="p_HP_Previous">当前生命值</param>
    /// <param name="p_Hurt">伤害</param>
    /// <returns>受到伤害后的生命值</returns>
    public static int HurtCalculate(int p_HP_Previous,int p_Hurt)
    {
        return (p_HP_Previous - p_Hurt < 0) ? 0 : p_HP_Previous - p_Hurt;
    }  
}
