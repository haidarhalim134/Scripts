using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class ApplyDPTDebuff : MonoBehaviour
    {
        public int DPT;
        public int charge;
        public GameObject debuffIcon;
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            void debuff(ActiveDebuff Data)
            {
                Data.target.TakeDamage(Calc.CalcDPT(this.DPT, caster, target));
                Data.charge-= 1;
            }
            string desc(ActiveDebuff Data)
            {
                return $" <b>{Mng.AbName}</b>\ndeal {this.DPT} damage when turn ends";
            }
            target.DebuffAddActive(target.buffDebuff.endTurnActivate, 
            new ActiveDebuff(Mng.AbName, charge, Data, caster, target, debuff, desc), debuffIcon);
        }
        public string Text(AbilityData data)
        {
            return $"Apply {this.charge} {this.DPT} DPT. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
