using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Control.Combat;
using Attributes.Abilities;
using DataContainer;


public class BaseAbility : MonoBehaviour
{
    public string closingDesc = ". ";
    protected AbilityManager Mng;
    protected StatProcessor Calc = new StatProcessor();
    virtual public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data) { yield return null; }
    virtual public string Text(AbilityData data, PlayerController caster, BaseCreature target) { return ""; }
    virtual protected void Awake()
    {
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
