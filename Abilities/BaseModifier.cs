using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class BaseModifier : BaseAbility
{
    public Targeting targeting;
    public ModType modType;
    public GameObject effect;
    new virtual public void Ability(BaseCreature caster, BaseCreature target, AbilityData data = null) { }
    new virtual public void Awake()
    {
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.modifier.modifier[modType].Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
