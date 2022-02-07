using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyStance : MonoBehaviour
    {
        [Tooltip("modifier")]
        public Stance stance;
        public Targeting targeting;
        public ModType modType;
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.DebuffsAddStance(stance);
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            return $"Enter <b>{this.stance}</b>. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
