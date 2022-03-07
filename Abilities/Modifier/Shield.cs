using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Shield : BaseModifier
    {
        [Header("shield")]
        public int shield = 10;
        override public void Ability(BaseCreature caster, BaseCreature target, AbilityData data = null)
        {
            BaseCreature to;
            if (targeting==Targeting.target)to = target;
            else to = caster;
            to.shield.Update(shield + Mng.GetLevelBonus(data).Shield);
            Animations.SpawnEffect(to.gameObject, effect);
        }
        override public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            int shil = shield + Mng.GetLevelBonus(data).Shield;
            return $"give {AbilityUtils.CalcColor(shield, shil)}{shil}{AbilityUtils.c} shield{closingDesc}";
        }
    }
}
