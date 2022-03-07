using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyPassiveDebuff : BaseModifier
    {
        [Tooltip("modifier")]
        public Debuffs type;
        public int charge;
        public string verb = "apply";
        public OverrideDesc Override = new OverrideDesc();
        override public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.DebuffsAddPassive(this.type, charge+Mng.GetLevelBonus(Data).Charge);
            Animations.SpawnEffect(to.gameObject, effect);
        }
        override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            int charg = charge + Mng.GetLevelBonus(data).Charge;
            if (Override.Override) return Override.desc;
            return $"{verb} {AbilityUtils.CalcColor(charge, charg)}{Math.Abs(charg)}{AbilityUtils.c} <b>{this.type}</b>{closingDesc}";
        }
    }
}
