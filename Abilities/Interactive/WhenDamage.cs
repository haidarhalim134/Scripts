using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class WhenDamage : MonoBehaviour
{
    public CardModifier whenApply;
    public int damageChange;
    AbilityManager mng;
    void Activate(AbilityData data)
    {
        data.tempAbData.damage+= damageChange + mng.GetLevelBonus(data).Damage1;
    }
    string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        int damage = damageChange + mng.GetLevelBonus(data).Damage1;
        return $" when {AbilityUtils.cMPastTense[whenApply]}, {(damage >0?"increase":"decrease")} damage by {AbilityUtils.CalcColor(damageChange, damage)}{Math.Abs(damage)}{AbilityUtils.c}";
    }
    void Awake()
    {
        mng = GetComponent<AbilityManager>();
        switch(whenApply)
        {
            case CardModifier.normal:
                mng.onUse.Add(Activate);
                break;
            case CardModifier.keep:
                mng.onKeep.Add(Activate);
                break;
        }
        mng.DescGrabber.Add(Text);
    }
}
