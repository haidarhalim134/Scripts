using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyStance : BaseModifier
    {
        [Tooltip("modifier")]
        public Stance stance;
        override public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.DebuffsAddStance(stance);
        }
        override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            return stance == Stance.noStance?"Exit stance":$"Enter <b>{this.stance}</b>{closingDesc}";
        }
    }
}
