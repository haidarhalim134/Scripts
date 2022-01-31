using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;

public class InstantAModifier : MonoBehaviour
{
    static StatProcessor Calc = new StatProcessor();
    AbilityManager Mng;
    public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
    {
        foreach (var key in Mng.modifier.modifier.Keys)
        {
            Mng.modifier.modifier[key].ForEach((abil)=>abil(caster,target,Data));
        }; 
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"";
    }
    void Awake()
    {
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
