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
        public AnimData casterAnim;
        public AnimData targetAnim;
        AbilityManager Mng;
        static StatProcessor Calc = new StatProcessor();
        public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
        {
            BaseCreature to;
            if (targeting == Targeting.target) to = target;
            else to = caster;
            void Hit()
            {
                Mng.ActivateModifier(ModType.preDamage, caster, to, Mng.GetLevelBonus(data));
                to.TakeDamage(Calc.CalcAttack(damage + Mng.GetLevelBonus(data).Damage, caster, to), caster, DamageSource.attack, throughArmor);
                Mng.ActivateModifier(ModType.postDamage, caster, to, Mng.GetLevelBonus(data));
                StartCoroutine(Animations.TowardsCenterAttack(to.gameObject, () => { }, () => { }, targetAnim));
                Animations.SpawnEffect(to.gameObject, effect);
            }
            Mng.ActivateModifier(ModType.preAttack, caster, to, Mng.GetLevelBonus(data));
            for (var i = 0; i< repetition+ Mng.GetLevelBonus(data).AttackRep;i++)
            {
                yield return StartCoroutine(Animations.TowardsCenterAttack(caster.gameObject, Hit, () => { }, casterAnim));
            }
            Mng.ActivateModifier(ModType.postAttack, caster, to, data);
        }
        public string Text(AbilityData data,PlayerController caster, BaseCreature target)
        {
            string rep = repetition + Mng.GetLevelBonus(data).AttackRep > 1?$" {repetition + Mng.GetLevelBonus(data).AttackRep} times":"";
            string ta = throughArmor?" through armor":"";
            if (caster!=null)
            {
                int calcdamage = Calc.CalcAttack(damage + Mng.GetLevelBonus(data).Damage, caster, target);
                string color = AbilityUtils.CalcColor(this.damage, calcdamage);
                return $"{verb} {color}{calcdamage}</color> damage{rep}{ta}{closingDesc}";
            }
            else return $"{verb} {damage + Mng.GetLevelBonus(data).Damage} damage{rep}{ta}{closingDesc}";
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
