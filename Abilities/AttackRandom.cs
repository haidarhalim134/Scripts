using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;

namespace Attributes.Abilities
{
    public class AttackRandom : MonoBehaviour
    {
        [Tooltip("instant")]
        public int damage = 10;
        public float delay;
        public int repetition;
        public GameObject effect;
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            Mng.modifier.modifier[ModType.preAttack].ForEach((abil) => abil(caster, target, data));
            for (var x = 0; x < repetition+ data.AttackRep; x++)
            {
                yield return new WaitForSeconds(this.delay);
                var to = CombatEngine.GetRandomTarget(target.TeamId);
                Mng.modifier.modifier[ModType.preDamage].ForEach((abil) => abil(caster, target, data));
                to.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, target), caster);
                Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, target, data));
                Animations.SpawnEffect(to.gameObject, effect);
            }
            Mng.modifier.modifier[ModType.postAttack].ForEach((abil) => abil(caster, target, data));
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            string rep = " " + repetition + data.AttackRep;
            if (caster != null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage to{(repetition>1?rep:" ")} random target. ";
            }
            else return $"deal {this.damage + data.Damage} damage to{(repetition > 1 ? rep : " ")} random target. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
