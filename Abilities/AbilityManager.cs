using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public List<Action<BaseCreature, BaseCreature, AbilityData>> ContainedAbilities = new List<Action<BaseCreature, BaseCreature, AbilityData>>();
        public List<Func<string>> DescGrabber = new List<Func<string>>();
        public string AbName; 
        public int cost;
        public int GoldCost;
        public AbTarget target;
        public string Desc;
        public string GetDesc()
        {
            this.Desc = "";
            foreach (Func<string> text in DescGrabber)
            {
                this.Desc+= text();
            }
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