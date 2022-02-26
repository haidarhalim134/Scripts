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
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            BaseCreature to;
            if (targeting==Targeting.target)to = target;
            else to = caster;
            to.shield.Update(this.shield + Data.Shield);
            Animations.SpawnEffect(to.gameObject, effect);
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            return $"give {this.shield} shield{closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.modifier.modifier[modType].Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}