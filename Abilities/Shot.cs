using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Shot : MonoBehaviour
    {
        public int damage = 10;
        public GameObject effect;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            void Hit()
            {
                target.TakeDamage(Calc.CalcAttack(this.damage + Data.Damage, caster, target));
                StartCoroutine(Animations.AwayCenterHit(target.gameObject, () => { }, 0.2f, 5f));
                Animations.SpawnEffect(target.gameObject, effect);
            }
            StartCoroutine(Animations.AwayCenterShot(caster.gameObject, Hit, 0.3f, 10f));
            
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            if (caster != null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage to enemy. ";
            }
            else return $"deal {this.damage + data.Damage} damage to enemy. ";
        }
        void Awake()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
