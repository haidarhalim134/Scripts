using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;

public class AbilityParent : MonoBehaviour
{
    public virtual IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        yield return null;
    }
    public virtual string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return "";
    }
}
