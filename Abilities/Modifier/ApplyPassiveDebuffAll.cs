using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using Control.Combat;

public class ApplyPassiveDebuffAll : MonoBehaviour
{
    [Tooltip("modifier")]
    public Debuffs type;
    public int charge;
    public ModType modType;
    public GameObject effect;
    public string verb = "apply";
    public string closingDesc = ". ";
    public OverrideDesc Override = new OverrideDesc();
    static StatProcessor Calc = new StatProcessor();
    AbilityManager Mng;
    public void Ability(BaseCreature caster, BaseCreature target, AbilityData Data = null)
    {
        CombatEngine.RegisteredCreature[target.TeamId].ForEach((creature) =>
        {
            creature.DebuffsAddPassive(this.type, this.charge);
            Animations.SpawnEffect(creature.gameObject, effect);
        });
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        if (Override.Override) return Override.desc;
        return $"{verb} {Math.Abs(this.charge)} <b>{this.type}</b> to All enemy{closingDesc}";
    }
    void Awake()
    {
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.modifier.modifier[modType].Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
