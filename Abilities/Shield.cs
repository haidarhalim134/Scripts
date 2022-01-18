using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Shield : MonoBehaviour
    {
        public int shield = 10;
        public GameObject effect;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
        {
            caster.shield.Update(this.shield + Data.Shield);
            Animations.SpawnEffect(caster.gameObject, effect);
        }
        public string Text()
        {
            return $"give {this.shield} shield";
        }
        void Awake()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
