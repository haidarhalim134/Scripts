using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyPassiveDebuff : MonoBehaviour
    {
        [Tooltip("modifier")]
        public Debuffs type;
        public int charge;
        public Targeting targeting;
        public ModType modType;
        public GameObject effect;
        public string verb = "apply";
        public string closingDesc = ". ";
        public OverrideDesc Override = new OverrideDesc();
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {   
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.DebuffsAddPassive(this.type, this.charge);
            Animations.SpawnEffect(to.gameObject, effect);
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            if (Override.Override)return Override.desc;
            return $"{verb} {Math.Abs(this.charge)} <b>{this.type}</b>{closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
