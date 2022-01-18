using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Attack : MonoBehaviour
    {
        public int damage = 10;
        public GameObject effect;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            StartCoroutine(Animations.TowardsCenterAttack(caster.gameObject, 
            ()=>{
                target.TakeDamage(Calc.CalcAttack(this.damage + Data.Damage, caster, target));
                StartCoroutine(Animations.TowardsCenterHit(target.gameObject,()=>{},0.2f,-5f));
                Animations.SpawnEffect(target.gameObject, effect);
                }));
            
        }
        public string Text()
        {
            return $"deal {this.damage} damage to enemy";
        }
        void Awake()
        {
            AbilityManager Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
