using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class ShieldBrokeActivate : BasePower
{
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff Data)
        {
            if (targeting == Targeting.caster)StartCoroutine(abilityMng.Activate(caster, target, Data.data));
            else StartCoroutine(abilityMng.Activate(caster, CombatEngine.GetRandomTarget(caster.EnemyId), Data.data));
        }
        string desc(ActiveDebuff Data)
        {
            return $"if your shield broke, " + abilityMng.GetDesc(Data.data, null, null)+closingDesc;
        }
        target.DebuffAddActive(target.buffDebuff.shieldBrokeActivate,
        new ActiveDebuff(Mng.AbName, hideCharge?int.MaxValue:data.Add(ability.Data).Sum(), data.Add(ability.Data), caster, target, debuff, desc), debuffIcon);
        yield return null;
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"everytime your shield broke, " + abilityMng.GetDesc(data, null, null)+closingDesc;
    }
}
