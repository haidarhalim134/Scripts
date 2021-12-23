using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control.Core;

namespace Attributes.Abilities
{
    public class AbilitiesRepository : MonoBehaviour
    {
        static StatProcessor Calc = new StatProcessor();
        private static bool Initialized = false;
        public static Dictionary<string, AbilityContainer> Abilities = new Dictionary<string, AbilityContainer>();
        private static void OneAttackF(BaseCreature caster, BaseCreature target)
        {
            target.TakeDamage(Calc.CalcAttack(10, caster));
        }
        private static void TwoAttackF(BaseCreature caster, BaseCreature target)
        {
            target.TakeDamage(Calc.CalcAttack(20, caster));
        }
        private static void ShieldUpF(BaseCreature caster, BaseCreature target = null)
        {
            caster.GiveShield(5);
        }
        // assing each ability to a static variable       
        public static AbilityContainer OneAttack = new AbilityContainer 
        {name = "OneAttack",cost = 1, target = 2, ability = OneAttackF};
        public static AbilityContainer TwoAttack = new AbilityContainer 
        {name = "TwoAttack",cost = 2, target = 2, ability = TwoAttackF};
        public static AbilityContainer ShieldUp = new AbilityContainer 
        {name = "ShieldUp",cost = 1, target = 0, ability = ShieldUpF};
        // public static void Initialize()
        // {
        //     if (!Initialized)
        //     {
        //         Initialized = true;
        //     }
        // }
        /// <summary>get ability container by name, consider making dictionary if this is too slow</summary>
        public static AbilityContainer GetAbility(string name)
        {
            foreach (AbilityContainer Cont in AbilityContainer.AllInstance)
            {
                if (Cont.name == name)
                {
                    return Cont;
                }
            }
            Debug.Log(name+" not found");
            return null;
        }
    }
    public class AbilityContainer
    {
        /// <summary>used to refer container from outside C# script, should be initialized to a dict</summary>
        public string name { get; set; }
        /// <summary>stamina cost</summary>
        public int cost { get; set; }
        /// <summary>self 0;allies 1;enemy 2</summary>
        public int target { get; set; }
        public int StatPlusPerLevel { get; set; }
        /// <summary>refer to ability's function</summary>
        public Action<BaseCreature,BaseCreature> ability { get; set; }
        /// <summary>keep track of all ability</summary>
        public static List<AbilityContainer> AllInstance = new List<AbilityContainer>();
        public AbilityContainer()
        {
            AllInstance.Add(this);
        }
    }
}