using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public ListAbil ContainedAbilities = new ListAbil();
        public List<Func<AbilityData,PlayerController,BaseCreature, string>> DescGrabber = new List<Func<AbilityData, PlayerController, BaseCreature, string>>();
        public List<Action<AbilityData>> onKeep = new List<Action<AbilityData>>();
        public List<Action<AbilityData>> onUse = new List<Action<AbilityData>>();
        public Modifier modifier = new Modifier();
        [Tooltip("must give a non empty unique name")]
        public string AbName; 
        public int cost;
        public CardType type;
        public int GoldCost;
        [HideInInspector]
        public List<CardModifier> cardModifiers;
        public AbTarget target;
        [Tooltip("used for bot intention system")]
        public AbilityType[] types;
        [HideInInspector]
        public AbilityData intentionData;
        public string Desc;
        public void OnKeep(AbilityData data)
        {
            onKeep.ForEach((func)=>func(data));
        }
        public void OnUse(AbilityData data)
        {
            onUse.ForEach((func)=>func(data));
        }
        public string GetDesc(AbilityData data, PlayerController caster=null, BaseCreature target=null)
        {
            this.Desc = "";
            DescGrabber.ForEach((text)=>this.Desc+= text(data, caster, target));
            return this.Desc;
        }
        public int GetStaminaCost(AbilityData Data)
        {
            return this.cost+Data.Staminacost;
        }
        public AbilityData GetData(AbilityData data)
        {
            return intentionData.Add(data);
        }
        public int GetDamageDeal(AbilityData data, BaseCreature caster, BaseCreature target)
        {
            StatProcessor Calc = new StatProcessor();
            return Calc.CalcAttack(GetData(data).Damage, caster, target);
        }
        public IEnumerator Activate(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            caster.currTween = true;
            foreach (var abil in ContainedAbilities)
            {
                if (target.dead)break;
                yield return StartCoroutine(abil(caster, target, Data));
            }
            caster.currTween = false;
            OnUse(Data);
        }
        void Awake()
        {
            GetDesc(new AbilityData());
        }
    }
    public class Modifier
    {
        public Dictionary<ModType, ListMod> modifier = new Dictionary<ModType, ListMod>()
        {{ModType.preAttack, new ListMod()},{ModType.preDamage, new ListMod()},
        {ModType.postDamage, new ListMod()},{ModType.postAttack, new ListMod()}};
    }
    public class ListAbil : List<Func<BaseCreature, BaseCreature, AbilityData, IEnumerator>>
    {

    }
    public class ListMod : List<Action<BaseCreature, BaseCreature, AbilityData>>
    {

    }
}