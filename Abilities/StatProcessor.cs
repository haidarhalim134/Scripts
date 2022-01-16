using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class StatProcessor
    {
        public int CalcAttack(int BaseDamage, BaseCreature Caster)
        {
            if (Caster.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.vulnerable) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 1.5f);
            };
            if (Caster.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.weakened) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 0.75f);
            };
            return BaseDamage;
        }
    }
}

