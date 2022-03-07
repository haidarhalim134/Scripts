using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class CurrHealth : BaseAbility
{
    public GameObject debuffIcon;
    [Header("insert > 0")]
    public int healthLostPerStack = 1;
    public AbilityManager increaser;
    public AbilityManager decreaser;
    override public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff data)
        {
            int percentageLost = (int)Math.Floor(100 - data.caster.health.Curr * 100 / (float)data.caster.health.Max);
            int targetCharge = percentageLost / healthLostPerStack;
            int action = targetCharge - data.charge;
            for (var i = 0; i < Math.Abs(action); i++)
            {
                if (action > 0) StartCoroutine(increaser.Activate(data.caster, data.target, data.data));
                else StartCoroutine(decreaser.Activate(data.caster, data.target, data.data));
            };
            data.update(targetCharge - data.charge);
        }
        string desc(ActiveDebuff Data)
        {
            return $"For every {healthLostPerStack}% hp lost, {increaser.GetDesc(data)}";
        }
        if (target.DebuffHaveActive<ActiveDebuff>(target.buffDebuff.healthChangeActivate, Mng.AbName)==null)
        {
            ActiveDebuff active = new ActiveDebuff(Mng.AbName, 0, data, caster, target, debuff, desc, false);
            active.hideIndicator = true;
            debuff(active);
            active.hideIndicator = false;
            target.DebuffAddActive(target.buffDebuff.healthChangeActivate, active, debuffIcon);
        }
        yield return null;
    }
    override public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"For every {healthLostPerStack}% hp lost, {increaser.GetDesc(data)}";
    }
    override protected void Awake()
    {
        base.Awake();
        increaser = InGameContainer.GetInstance().SpawnAbilityPrefab(increaser.gameObject);
        decreaser = InGameContainer.GetInstance().SpawnAbilityPrefab(decreaser.gameObject);
    }
}
