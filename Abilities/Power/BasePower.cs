using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class BasePower : MonoBehaviour
{
    public GameObject debuffIcon;
    [Tooltip("since this target self, if caster target caster, else target random enemy")]
    public PowerTargeting targeting;
    public BotAbilityCont ability;
    public bool hideCharge;
    public string closingDesc = ". ";
    protected AbilityManager abilityMng;
    protected AbilityManager Mng;
    protected StatProcessor Calc = new StatProcessor();
    virtual public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data) {yield return null;}
    virtual public string Text(AbilityData data, PlayerController caster, BaseCreature target) {return "" ;}
    void Awake()
    {
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability.Ability);
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
    public void activate(ActiveDebuff data)
    {
        if (targeting == PowerTargeting.caster) StartCoroutine(abilityMng.Activate(data.caster, data.caster, Mng.GetLevelBonus(data.data)));
        else if (targeting == PowerTargeting.target) StartCoroutine(abilityMng.Activate(data.caster, data.target, Mng.GetLevelBonus(data.data)));
        else StartCoroutine(abilityMng.Activate(data.caster, CombatEngine.GetRandomTarget(data.caster.EnemyId), Mng.GetLevelBonus(data.data)));
    }
}
