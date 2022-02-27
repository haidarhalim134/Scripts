using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class AttackPlayActivate : BasePower
{
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff data, AbilityContainer lastPlayed)
        {
            if (lastPlayed.GetManager().type == CardType.attack)
            {
                if (targeting == Targeting.caster) StartCoroutine(abilityMng.Activate(caster, target, data.data));
                else StartCoroutine(abilityMng.Activate(caster, CombatEngine.GetRandomTarget(caster.EnemyId), data.data));
            }
        }
        string desc(ActiveDebuff Data)
        {
            return $"if your shield broke, " + abilityMng.GetDesc(Data.data, null, null) + closingDesc;
        }
        target.DebuffAddActive(target.buffDebuff.attackPlayActivate,
        new ActiveDebuffCardPlay(Mng.AbName, hideCharge ? int.MaxValue : data.Add(ability.Data).Sum(), data.Add(ability.Data), caster, target, debuff, desc), debuffIcon);
        yield return null;
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"everytime your play attack card, " + abilityMng.GetDesc(data.Add(ability.Data), null, null) + closingDesc;
    }
}
