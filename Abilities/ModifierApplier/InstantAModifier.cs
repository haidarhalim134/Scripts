using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;

public class InstantAModifier : MonoBehaviour
{
    [Tooltip("Apply modifier, instant")]
    static StatProcessor Calc = new StatProcessor();
    AbilityManager Mng;
    public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        foreach (var key in Mng.modifier.modifier.Keys)
        {
            Mng.ActivateModifier(key, caster, target, data);
        }
        yield return new WaitForSeconds(0);
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
