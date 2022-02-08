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
            IEnumerator hit()
            {
                for (var x = 0;x<repetition;x++)
                {
                    yield return new WaitForSeconds(this.delay);
                    var to = CombatEngine.GetRandomTarget(target.TeamId);
                    to.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, target), caster);
                    Animations.SpawnEffect(to.gameObject, effect);
                }
            }
            yield return StartCoroutine(hit());
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            // TODO: refactor this
            if (caster != null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"deal {color}{calcdamage}</color> damage {(repetition>1?$"to {repetition} random target":"to random target")}. ";
            }
            else return $"deal {this.damage + data.Damage} damage {(repetition > 1 ? $"to {repetition} random target" : "to random target")}. ";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
