using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Attack : MonoBehaviour
    {
        [Tooltip("Apply modifier, not instant")]
        public int damage = 10;
        public GameObject effect;
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public void Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            void Hit()
            {
                Mng.modifier.modifier[ModType.preDamage].ForEach((abil)=>abil(caster,target,data));
                target.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, target), caster);
                Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, target, data));
                StartCoroutine(Animations.AwayCenterHit(target.gameObject, () => { }, 0.2f, 5f));
                Animations.SpawnEffect(target.gameObject, effect);
            }
            void postHit()
            {
                Mng.modifier.modifier[ModType.postAttack].ForEach((abil) => abil(caster, target, data));
            }
            Mng.modifier.modifier[ModType.preAttack].ForEach((abil) => abil(caster, target, data));
            StartCoroutine(Animations.TowardsCenterAttack(caster.gameObject, Hit,postHit));
            
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            if (caster!=null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage. ";
            }
            else return $"deal {this.damage + data.Damage} damage. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
