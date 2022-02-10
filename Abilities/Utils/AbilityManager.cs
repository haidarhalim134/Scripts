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
        public Modifier modifier = new Modifier();
        [Tooltip("must give a non empty unique name")]
        public string AbName; 
        public int cost;
        public int GoldCost;
        public AbTarget target;
        [Tooltip("used for bot intention system")]
        public SpellType[] types;
        public AbilityData intentionData;
        public string Desc;
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
        public IEnumerator Activate(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            foreach (var abil in ContainedAbilities)
            {
                if (target.dead)break;
                yield return StartCoroutine(abil(caster, target, Data));
            }
        }
        void Awake()
        {
            GetDesc(new AbilityData());
        }
    }
    public enum AbTarget{self, allies, enemy}
    public enum ModType{preAttack,preDamage,postDamage,postAttack}
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