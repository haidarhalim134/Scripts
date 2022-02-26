using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyDPTDebuff : MonoBehaviour
    {
        [Tooltip("modifier")]
        public int DPT;
        public int charge;
        public ModType modType;
        public GameObject debuffIcon;
        public string closingDesc = ". ";
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            void debuff(ActiveDebuff Data)
            {
                Data.target.TakeDamage(Calc.CalcDPT(this.DPT, caster, target), caster, DamageSource.skill);
                Data.update(-1);
            }
            string desc(ActiveDebuff Data)
            {
                return $" <b>{Mng.AbName}</b>\ndeal {Calc.CalcDPT(this.DPT, caster, target)} damage when turn ends";
            }
            target.DebuffAddActive(target.buffDebuff.endTurnActivate, 
            new ActiveDebuff(Mng.AbName, charge, Data, caster, target, debuff, desc), debuffIcon);
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            return $"Apply {this.charge} {this.DPT} DPT{closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}