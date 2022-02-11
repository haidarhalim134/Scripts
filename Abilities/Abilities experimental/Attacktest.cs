using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class Attacktest : MonoBehaviour
    {
        [Tooltip("Apply modifier, not instant")]
        public int damage = 10;
        public int repetition = 1;
        public GameObject effect;
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            void Hit()
            {
                Mng.modifier.modifier[ModType.preDamage].ForEach((abil) => abil(caster, target, data));
                target.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, target), caster);
                Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, target, data));
                StartCoroutine(Animations.AwayCenterHit(target.gameObject, () => { }, 0.2f, 5f));
                Animations.SpawnEffect(target.gameObject, effect);
            }
            Mng.modifier.modifier[ModType.preAttack].ForEach((abil) => abil(caster, target, data));
            for (var i = 0; i < repetition + data.AttackRep; i++)
            {
                yield return StartCoroutine(Animations.TowardsCenterAttack(caster.gameObject, Hit, () => { }));
            }
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            string rep = $"{repetition + data.AttackRep} times";
            if (caster != null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage {(repetition > 1 ? rep : "")}. ";
            }
            else return $"deal {this.damage + data.Damage} damage {(repetition > 1 ? rep : "")}. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.intentionData.Damage = damage;
            Mng.intentionData.AttackRep = repetition;
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
