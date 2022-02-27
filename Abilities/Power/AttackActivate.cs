using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class AttackActivate : BasePower
{
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        yield return null;
    }
}
