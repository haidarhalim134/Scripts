using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class OneAttack : MonoBehaviour
    {
        public int damage = 10;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, string GUID = null)
        {
            target.TakeDamage(Calc.CalcAttack(this.damage, caster));
        }
        public string Text()
        {
            return $"deal {this.damage} damage to enemy";
        }
        void Start()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
