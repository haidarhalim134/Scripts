using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;
using Attributes.Abilities;
using DataContainer;

public class ShieldBrokeActivate : MonoBehaviour
{
    public GameObject debuffIcon;
    [Tooltip("since this target self, if caster target caster, else target random enemy")]
    public Targeting targeting;
    public GameObject ability;
    AbilityManager abilityMng;
    AbilityManager Mng;
    static StatProcessor Calc = new StatProcessor();
    public IEnumerator Ability(BaseCreature caster, BaseCreature target, AbilityData data)
    {
        void debuff(ActiveDebuff Data)
        {
            Debug.Log("activated");
            if (targeting == Targeting.caster)StartCoroutine(abilityMng.Activate(caster, target, data));
            Data.charge -= 1;
        }
        string desc(ActiveDebuff Data)
        {
            return $"if your shield broke, " + abilityMng.GetDesc(data, null, null);
        }
        target.DebuffAddActive(target.buffDebuff.shieldBrokeActivate,
        new ActiveDebuff(Mng.AbName, -1, data, caster, target, debuff, desc), debuffIcon);
        yield return null;
    }
    public string Text(AbilityData data, PlayerController caster, BaseCreature target)
    {
        return $"if your shield broke, " + abilityMng.GetDesc(data, null, null);
    }
    void Awake()
    {
        abilityMng = InGameContainer.GetInstance().SpawnAbilityPrefab(ability);
        Mng = gameObject.GetComponent<AbilityManager>();
        Mng.ContainedAbilities.Add(this.Ability);
        Mng.DescGrabber.Add(this.Text);
    }
}
