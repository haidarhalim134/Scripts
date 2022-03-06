using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class GainStamina : MonoBehaviour
    {
        [Tooltip("modifier")]
        public int stamina;
        public Targeting targeting;
        public ModType modType;
        public GameObject effect;
        public string closingDesc = ". ";
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData data = null)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            to.stamina.Update(stamina + Mng.GetLevelBonus(data).BonusStamina);
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            int stam = stamina + Mng.GetLevelBonus(data).BonusStamina;
            return $"Gain {AbilityUtils.CalcColor(stamina, stam)}{stam}{AbilityUtils.c} stamina{closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
