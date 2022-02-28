using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class GetPassiveDebuffActivate : BasePower
{
    Debuffs whenActivate;
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff data, Debuffs lastGet)
        {
            if (lastGet == whenActivate)
            {
                if (targeting == Targeting.caster) StartCoroutine(abilityMng.Activate(caster, target, data.data));
                else StartCoroutine(abilityMng.Activate(caster, CombatEngine.GetRandomTarget(caster.EnemyId), data.data));
            }
        }
        string desc(ActiveDebuff Data)
        {
            return $"everytime your gain {whenActivate}, " + abilityMng.GetDesc(Data.data, null, null) + closingDesc;
        }
        target.DebuffAddActive(target.buffDebuff.passiveGetActivate,
        new ActiveDebuffGetPassiveDebuff(Mng.AbName, hideCharge ? int.MaxValue : data.Add(ability.Data).Sum(), data.Add(ability.Data), caster, target, debuff, desc), debuffIcon);
        yield return null;
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"everytime your gain {whenActivate}, " + abilityMng.GetDesc(data.Add(ability.Data), null, null) + closingDesc;
    }
}
