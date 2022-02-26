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
    void Activate(AbilityData data)
    {
        data.tempAbData.Damage+= damageChange;
    }
    void Awake()
    {
        AbilityManager mng = GetComponent<AbilityManager>();
        switch(whenApply)
        {
            case CardModifier.normal:
                mng.onUse.Add(Activate);
                break;
            case CardModifier.keep:
                mng.onKeep.Add(Activate);
                break;
        }
    }
}
