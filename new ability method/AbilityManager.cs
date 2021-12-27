using System;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public List<Action<BaseCreature,BaseCreature>> ContainedAbilities;
        public List<Func<string>> DescGrabber; 
        public int Cost;
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
        public void Activate(BaseCreature caster, BaseCreature target)
        {
            foreach(Action<BaseCreature, BaseCreature> abil in ContainedAbilities)
            {
                abil(caster, target);
            }
        }
    }
}