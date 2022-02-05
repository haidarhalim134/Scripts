using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyPassiveDebuff : MonoBehaviour
    {
        public Debuffs type;
        public int charge;
        public ModType modType;
        public GameObject effect;
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            target.DebuffsAddPassive(this.type, this.charge);
            Animations.SpawnEffect(caster.gameObject, effect);
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            return $"Apply {this.charge} <b>{this.type}</b>. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
