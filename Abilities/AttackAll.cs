using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;
using Control.Combat;

public class AttackAll : MonoBehaviour
{
    [Tooltip("Apply modifier, instant, pre attack and post attack not applied")]
    public int damage = 10;
    public int repetition = 1;
    public GameObject effect;
    AbilityManager Mng;
    static StatProcessor Calc = new StatProcessor();
    public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        CombatEngine.RegisteredCreature[target.TeamId].ForEach((creature)=>{
            Mng.modifier.modifier[ModType.preDamage].ForEach((abil) => abil(caster, creature, data));
            creature.TakeDamage(Calc.CalcAttack(this.damage + data.Damage, caster, creature), caster);
            Mng.modifier.modifier[ModType.postDamage].ForEach((abil) => abil(caster, creature, data));
            Animations.SpawnEffect(creature.gameObject, effect);
        });
        yield return null;
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        string rep = $"{repetition + data.AttackRep} times";
        if (caster != null)
        {
            int calcdamage = Calc.CalcAttack(this.damage + data.Damage, caster, target);
            string color = AbilityUtils.CalcColor(this.damage, calcdamage);
            return $"deal {color}{calcdamage}</color> damage to All enemies. ";
        }
        else return $"deal {this.damage + data.Damage} damage to All enemies. ";
    }
    void Awake()
    {
        Mng.intentionData.Damage = damage;
        Mng.intentionData.AttackRep = repetition;
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
