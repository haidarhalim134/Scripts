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
        public int repetition = 1;
        public Targeting targeting = Targeting.target;
        public bool throughArmor;
        public GameObject effect;
        public string verb = "deal";
        public string closingDesc = ". ";
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            void Hit()
            {
                Mng.modifier.modifier[ModType.preDamage].ForEach((abil)=>abil(caster,to,data));
                to.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, to), caster, DamageSource.attack, throughArmor);
                Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, to, data));
                StartCoroutine(Animations.AwayCenterHit(to.gameObject, () => { }, 0.2f, 5f));
                Animations.SpawnEffect(to.gameObject, effect);
            }
            Mng.modifier.modifier[ModType.preAttack].ForEach((abil) => abil(caster, to, data));
            for (var i = 0; i< repetition+ data.AttackRep;i++)
            {
                yield return StartCoroutine(Animations.TowardsCenterAttack(caster.gameObject, Hit, () => { }));
            }
            Mng.modifier.modifier[ModType.postAttack].ForEach((abil) => abil(caster, to, data));
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            string rep = repetition + data.AttackRep> 1?$"{repetition+data.AttackRep} times":"";
            string ta = throughArmor?"through armor":"";
            if (caster!=null)
            {
                int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"{verb} {color}{calcdamage}</color> damage {rep} {ta} {closingDesc}";
            }
            else return $"{verb} {this.damage + data.Damage} damage {rep} {ta} {closingDesc}";
        }
        void Awake()
        {
            Mng = gameObject.GetComponent<AbilityManager>();
            Mng.intentionData.Damage += damage;
            Mng.intentionData.AttackRep += repetition;
            Mng.ContainedAbilities.Add(this.Ability);
            Mng.DescGrabber.Add(this.Text);
        }
    }
}
