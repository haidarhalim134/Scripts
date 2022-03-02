using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class StatProcessor
    {
        public int CalcAttack(int BaseDamage, BaseCreature caster, BaseCreature target)
        {
            if (caster != null)
            {
                var pDebuff = caster.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.strength);
                if (pDebuff != null)
                {
                    BaseDamage+= pDebuff.charge;
                }
            }
            if (target != null)
            {
                var pDebuff = target.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.wound);
                if (pDebuff != null)
                {
                    BaseDamage += pDebuff.charge;
                }
            }
            if (target != null)
            if (target.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.vulnerable) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 1.5f);
            }
            if (target != null)
            if (target.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.marked) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 2f);
            };
            if (caster != null)
            if (caster.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.weakened) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 0.75f);
            };
            if (caster != null)
            if (caster.buffDebuff.stance.stance == Stance.rage)
            {
                BaseDamage*= 2;
            }
            if (target != null)
            if (target.buffDebuff.stance.stance == Stance.rage)
            {
                BaseDamage *= 2;
            }
            return BaseDamage;
        }
        public int CalcDPT(int BaseDamage, BaseCreature Caster, BaseCreature Target)
        {
            if (Target.buffDebuff.passiveDebuffs.Find((cont) => cont.debuff == Debuffs.vulnerable) != null)
            {
                BaseDamage = (int)Math.Floor(BaseDamage * 1.5f);
            };
            return BaseDamage;
        }
    }
}

