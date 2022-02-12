using System;
using System.Collections.Generic;
using UnityEngine;
using DataContainer;

namespace Attributes.Abilities
{
    public class AbilityUtils
    {
        public static string g = "<color=#29b400>";
        public static string r = "<color=\"red\">";
        public static string b = "<b>";
        public static string CalcColor(int basenumber, int calcnumber)
        {
            if (basenumber == calcnumber)return "";
            else if (basenumber < calcnumber)return g;
            else return r;
        }
    }
    [Serializable]
    public class AbilityContainer
    {
        public AbilityData Data;
        public string name;
        public AbilityManager GetManager()
        {
            GameObject Object = GameObject.Find(name);
            if (Object == null)
            {
                InGameContainer.GetInstance().SpawnAbilityPrefab(name);
            }
            return GameObject.Find(name).GetComponent<AbilityManager>();
        }
    }
    [Serializable]
    public class BotAbilityCont
    {
        public GameObject Ability;
        public AbilityData Data;
        public AbilityContainer ToNormalContainer()
        {
            return new AbilityContainer(){name=Ability.GetComponent<AbilityManager>().AbName, Data=Data};
        }
    }
    [Serializable]
    public class AbilityData
    {
        public int Level;
        public int Damage;
        public int Shield;
        public int Staminacost;
        public int AttackRep;
        public AbilityData Add(AbilityData data)
        {
            var res = new AbilityData(){
                Level = this.Level + data.Level,
                Damage = this.Damage + data.Damage,
                Shield = this.Shield + data.Shield,
                Staminacost = this.Staminacost + data.Staminacost,
                AttackRep = this.AttackRep + data.AttackRep
            };
            return res;
        }
    }
    public enum Debuffs{vulnerable, weakened}
    public enum Stance{rage, excited, noStance}
    public enum Targeting{caster,target}
    public enum AbilityType{attack,buff,debuff,shield}
}