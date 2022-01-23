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
            void Debuff(ActiveDebuff Data)
            {
                Data.target.TakeDamage(this.DPT);
            }
            target.DebuffAddActive(target.buffDebuff.endTurnActivate, 
            new ActiveDebuff(Mng.AbName, charge, Data, caster, target, Debuff));
        }
        public string Text()
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
