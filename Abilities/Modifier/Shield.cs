using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Shield : MonoBehaviour
    {
        [Tooltip("modifier")]
        public int shield = 10;
        public Targeting targeting;
        public ModType modType;
        public GameObject effect;
        public string closingDesc = ". ";
        static StatProcessor Calc = new StatProcessor();
        AbilityManager Mng;
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData data = null)
        {
            BaseCreature to;
            if (targeting==Targeting.target)to = target;
            else to = caster;
            to.shield.Update(shield + Mng.GetLevelBonus(data).Shield);
            Animations.SpawnEffect(to.gameObject, effect);
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            int shil = shield + Mng.GetLevelBonus(data).Shield;
            return $"give {AbilityUtils.CalcColor(shield, shil)}{shil}{AbilityUtils.c} shield{closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
