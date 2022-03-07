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
        [Header("damage, attackrep")]
        public int damage = 10;
        public float delay;
        public int repetition;
        public GameObject effect;
        public string verb = "deal";
        public string closingDesc = ". ";
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            var to = CombatEngine.GetRandomTarget(target.TeamId);
            Mng.ActivateModifier(ModType.preAttack, caster, to, data);
            for (var x = 0; x < repetition + Mng.GetLevelBonus(data).AttackRep; x++)
            {
                yield return new WaitForSeconds(this.delay);
                Mng.ActivateModifier(ModType.preDamage, caster, to, data);
                to.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, to), caster, DamageSource.attack);
                Mng.ActivateModifier(ModType.postDamage, caster, to, data);
                Animations.SpawnEffect(to.gameObject, effect);
            }
            Mng.ActivateModifier(ModType.postAttack, caster, to, data);
        }
        public string Text(AbilityData data, PlayerController caster, BaseCreature target)
        {
            string rep = " " + repetition + data.AttackRep;
            int based = this.damage + Mng.GetLevelBonus(data).Damage;
            if (caster != null)
            {
                based = Calc.CalcAttack(based, caster, target);
            }
            return $"{verb} {AbilityUtils.CalcColor(damage, based)}{based}{AbilityUtils.c} damage to{(repetition > 1 ? rep : " ")} random target{closingDesc}";
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
