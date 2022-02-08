using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class IfStance : MonoBehaviour
{
    public Stance stance;
    public GameObject ability;
    AbilityManager abilityMng;
    AbilityManager Mng;
    public void Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        if (caster.buffDebuff.stance.stance == stance)
        {
            abilityMng.Activate(caster, target, data);
        }
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"if {stance}, "+abilityMng.GetDesc(data, caster, target);
    }
    void Awake()
    {
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability);
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
