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
        public string AbName; 
        public int cost;
        public int GoldCost;
        public AbTarget target;
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
        public void Activate(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            foreach (var abil in ContainedAbilities)
            {
                abil(caster, target, Data);
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
        public Dictionary<ModType, ListAbil> modifier = new Dictionary<ModType, ListAbil>()
        {{ModType.preAttack, new ListAbil()},{ModType.preDamage, new ListAbil()},
        {ModType.postDamage, new ListAbil()},{ModType.postAttack, new ListAbil()}};
    }
    public class ListAbil : List<Action<BaseCreature, BaseCreature, AbilityData>>
    {

    }
}