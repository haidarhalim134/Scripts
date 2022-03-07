using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class GainStamina : BaseModifier
    {
        [Header("bonusStamina")]
        public int stamina;
        override public void Ability(BaseCreature caster, BaseCreature target, AbilityData data = null)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.stamina.Update(stamina + Mng.GetLevelBonus(data).BonusStamina);
        }
        override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            int stam = stamina + Mng.GetLevelBonus(data).BonusStamina;
            return $"Gain {AbilityUtils.CalcColor(stamina, stam)}{stam}{AbilityUtils.c} stamina{closingDesc}";
        }
    }
}
