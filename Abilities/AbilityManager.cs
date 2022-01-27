using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public List<Action<BaseCreature, BaseCreature, AbilityData>> ContainedAbilities = new List<Action<BaseCreature, BaseCreature, AbilityData>>();
        public List<Func<AbilityData, string>> DescGrabber = new List<Func<AbilityData, string>>();
        public string AbName; 
        public int cost;
        public int GoldCost;
        public AbTarget target;
        public string Desc;
        public string GetDesc(AbilityData data)
        {
            this.Desc = "";
            DescGrabber.ForEach((text)=>this.Desc+= text(data));
            return this.Desc;
        }
        public int GetStaminaCost(AbilityData Data)
        {
            return this.cost+Data.Staminacost;
        }
        public void Activate(BaseCreature caster, BaseCreature target, AbilityData Data)
        {
            foreach(Action<BaseCreature, BaseCreature, AbilityData> abil in ContainedAbilities)
            {
                abil(caster, target, Data);
            }
        }
    }
    public enum AbTarget{self, allies, enemy}
}