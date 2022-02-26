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
        public string closingDesc = ". ";
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            var to = CombatEngine.GetRandomTarget(target.TeamId);
            Mng.modifier.modifier[ModType.preAttack].ForEach((abil) => abil(caster, to, data));
            for (var x = 0; x < repetition+ data.AttackRep; x++)
            {
                yield return new WaitForSeconds(this.delay);
                Mng.modifier.modifier[ModType.preDamage].ForEach((abil) => abil(caster, to, data));
                to.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, to), caster, DamageSource.attack);
                Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, to, data));
                Animations.SpawnEffect(to.gameObject, effect);
            }
            Mng.modifier.modifier[ModType.postAttack].ForEach((abil) => abil(caster, to, data));
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            string rep = " " + repetition + data.AttackRep;
            if (caster != null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage to{(repetition>1?rep:" ")} random target{closingDesc}";
            }
            else return $"deal {this.damage + data.Damage} damage to{(repetition > 1 ? rep : " ")} random target{closingDesc}";
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