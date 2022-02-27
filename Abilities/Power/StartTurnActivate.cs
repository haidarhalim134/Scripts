using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;

public class StartTurnActivate : MonoBehaviour
{
    public GameObject debuffIcon;
    [Tooltip("since this target self, if caster target caster, else target random enemy")]
    public Targeting targeting;
    /// <summary>only fill one field for the ability data</summary>
    public BotAbilityCont ability;
    public bool hideCharge;
    public string closingDesc = ". ";
    AbilityManager abilityMng;
    AbilityManager Mng;
    static StatProcessor Calc = new StatProcessor();
    public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff Data)
        {
            if (targeting == Targeting.caster) StartCoroutine(abilityMng.Activate(caster, target, Data.data));
            else StartCoroutine(abilityMng.Activate(caster, CombatEngine.GetRandomTarget(caster.EnemyId), Data.data));
        }
        string desc(ActiveDebuff Data)
        {
            return $"at the start of your turn, " + abilityMng.GetDesc(Data.data, null, null) + closingDesc;
        }
        target.DebuffAddActive(target.buffDebuff.startTurnActivate,
        new ActiveDebuff(Mng.AbName, hideCharge ? int.MaxValue : data.Add(ability.Data).Sum(), data.Add(ability.Data), caster, target, debuff, desc), debuffIcon);
        yield return null;
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"at the start of your turn, " + abilityMng.GetDesc(data.Add(ability.Data), null, null) + closingDesc;
    }
    void Awake()
    {
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability.Ability);
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
