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
        // TODO: either copy the container or create non serializable field called temp
        public AbilityData Copy()
        {
            return new AbilityData(){ Level=Level, Damage=Damage, Shield=Shield };
        }
    }
    public enum Debuffs{vulnerable, weakened}
    public enum Stance{rage, excited}
    public enum Targeting{caster,target}
}