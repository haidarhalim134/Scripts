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
    public string verb = "deal";
    public string closingDesc = ". ";
    AbilityManager Mng;
    static StatProcessor Calc = new StatProcessor();
    public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        for (var i = 0;i < repetition + Mng.GetLevelBonus(data).AttackRep;i++)
        CombatEngine.RegisteredCreature[target.TeamId].ForEach((creature)=>{
            Mng.ActivateModifier(ModType.preDamage, caster, creature, Mng.GetLevelBonus(data));
            creature.TakeDamage(Calc.CalcAttack(damage + Mng.GetLevelBonus(data).Damage, caster, creature), caster, DamageSource.attack);
            Mng.ActivateModifier(ModType.postDamage, caster, creature, Mng.GetLevelBonus(data));
            Animations.SpawnEffect(creature.gameObject, effect);
        });
        yield return null;
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        int finalrep = repetition + Mng.GetLevelBonus(data).AttackRep;
        string rep = finalrep > 0?$" {AbilityUtils.CalcColor(repetition, finalrep)}{finalrep}{AbilityUtils.c} times":"";
        int based = damage + Mng.GetLevelBonus(data).Damage;
        if (caster != null)
        {
            based = Calc.CalcAttack(based, caster, target);
        }
        return $"{verb} {AbilityUtils.CalcColor(damage, based)}{based}{AbilityUtils.c} damage to All enemies{rep}{closingDesc}";
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
