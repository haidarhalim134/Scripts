using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public List<Action<BaseCreature, BaseCreature, string>> ContainedAbilities = new List<Action<BaseCreature, BaseCreature, string>>();
        public List<Func<string>> DescGrabber = new List<Func<string>>(); 
        public int cost;
        public int target;
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
        public void Activate(BaseCreature caster, BaseCreature target, string GUID = null)
        {
            foreach(Action<BaseCreature, BaseCreature, string> abil in ContainedAbilities)
            {
                abil(caster, target, GUID);
            }
        }
    }
}